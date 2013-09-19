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
			while (compilation_requires_more_work())
			{
				data = compile_one_step(data);
			}
			return data;
		}

		public bool compilation_requires_more_work()
		{
			return _local_passes.Count > 0;
		}

		[NotNull]
		public T compile_one_step([NotNull] T data)
		{
			var ready_passes = _local_passes.Where(all_preconditions_are_satisfied)
				.ToList();
			if (ready_passes.Count == 0)
			{
				throw new InvalidOperationException(
					"Compilation will never finish. There are remaining passes to execute, but none of them can be executed as none have all their conditions met. Please fix your set of passes.");
			}
			var new_data = ready_passes.Aggregate(data, apply_one_pass(_current_state));
			ProgrammerError.report_if(new_data != null,
				"Result of a compilation pass was null. Passes should always return a value.");
			_local_passes.RemoveAll(ready_passes.Contains);
			return new_data;
		}

		private bool all_preconditions_are_satisfied([NotNull] NanoPass<T> p)
		{
			return p.requires.Aggregate(true,
				// ReSharper disable AssignNullToNotNullAttribute
				(prev, cond) => prev && _current_state.Contains(cond));
			// ReSharper restore AssignNullToNotNullAttribute
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
