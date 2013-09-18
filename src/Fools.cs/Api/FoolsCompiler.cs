// FoolsCompiler.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
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
		[NotNull] private readonly List<NanoPass<T>> _local_passes;
		[NotNull] private readonly NonNullList<AstStateCondition> _current_state = new NonNullList<AstStateCondition>();

		public FoolsCompiler([NotNull] ReadOnlyCollection<NanoPass<T>> local_passes)
		{
			_local_passes = local_passes.ToList();
		}

		public FoolsCompiler() : this(NanoPass<T>.all) {}

		[NotNull]
		public T compile([NotNull] T data)
		{
			while (_local_passes.Count > 0)
			{
				var ready_passes = _local_passes.Where(p => {
					Debug.Assert(p != null, "p != null");
					return p.requires.Aggregate(true,
						(prev, cond) => {
							Debug.Assert(cond != null, "cond != null");
							return prev && _current_state.Contains(cond);
						});
				})
					.ToList();
				if (ready_passes.Count == 0)
				{
					throw new InvalidOperationException(
						"Compilation will never finish. There are remaining passes to execute, but none of them can be executed as none have all their conditions met. Please fix your set of passes.");
				}
				var new_data = ready_passes.Aggregate(data, apply_one_pass(_current_state));
				Debug.Assert(new_data != null, "new_data != null");
				data = new_data;
				_local_passes.RemoveAll(ready_passes.Contains);
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
