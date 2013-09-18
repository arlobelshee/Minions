// FoolsCompiler.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Fools.cs.TransformAst;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	[PublicAPI]
	public class FoolsCompiler<T> where T : class
	{
		[NotNull] private readonly ReadOnlyCollection<NanoPass<T>> _local_passes;

		public FoolsCompiler([NotNull] ReadOnlyCollection<NanoPass<T>> local_passes)
		{
			_local_passes = local_passes;
		}

		public FoolsCompiler() : this(NanoPass<T>.all) {}

		[NotNull]
		public T compile([NotNull] T data)
		{
			return _process(data, _local_passes);
		}

		[NotNull]
		private static T _process([NotNull] T data, [NotNull] ReadOnlyCollection<NanoPass<T>> passes)
		{
			var current_state = new NonNullList<AstStateCondition>();
			var remaining_passes = passes;
			while (remaining_passes.Count > 0)
			{
				var ready_passes = remaining_passes.Where(p => {
					Debug.Assert(p != null, "p != null");
					return p.requires.Aggregate(true,
						(prev, cond) => {
							Debug.Assert(cond != null, "cond != null");
							return prev && current_state.Contains(cond);
						});
				})
					.ToList();
				if (ready_passes.Count == 0)
				{
					throw new InvalidOperationException(
						"Compilation will never finish. There are remaining passes to execute, but none of them can be executed as none have all their conditions met. Please fix your set of passes.");
				}
				var new_data = ready_passes.Aggregate(data, apply_one_pass(current_state));
				Debug.Assert(new_data != null, "new_data != null");
				data = new_data;
				remaining_passes = remaining_passes.Where(p => !ready_passes.Contains(p))
					.ToList()
					.AsReadOnly();
			}
			return data;
		}

		[NotNull]
		private static Func<T, NanoPass<T>, T> apply_one_pass([NotNull] NonNullList<AstStateCondition> current_state)
		{
			return (current, pass) => {
				Debug.Assert(pass != null, "pass != null");
				Debug.Assert(current != null, "current != null");
				return pass.run(current, current_state.Add);
			};
		}
	}
}
