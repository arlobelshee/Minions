// TransformAstIntoExecutionModel.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using Fools.cs.AST;
using Fools.cs.Api;
using Fools.cs.TransformAst;
using Fools.cs.Utilities;
using NUnit.Framework;

namespace Fools.cs.Tests.Compilation
{
	[TestFixture]
	public class TransformAstIntoExecutionModel
	{
		[Test]
		public void compiler_should_locate_all_passes()
		{
			NanoPass<ProgramFragment>.all.Should()
				.Contain(pass => pass is CreateTestMissions);
		}

		[Test]
		public void compiler_should_run_passes_in_the_correct_order()
		{
			const string starting_block = "This minion";
			var test_subject =
				new FoolsCompiler<string>(local_passes(
					new AppendToName("that runs fast", requires("species identified"), grants()),
					new AppendToName("is a dog", requires(), grants("species identified"))));
			var transformed_block = test_subject.compile(starting_block);
			transformed_block.Should()
				.Be("This minion is a dog that runs fast");
		}

		[Test]
		public void uncompletable_language_should_throw_exception()
		{
			const string data = "This minion";
			var test_subject =
				new FoolsCompiler<string>(local_passes(new AppendToName("that runs fast", requires("is a dog"), grants())));
			Action compilation = () => test_subject.compile(data);
			compilation.ShouldThrow<InvalidOperationException>()
				.WithMessage(
					"Compilation will never finish. There are remaining passes to execute, but none of them can be executed as none have all their conditions met. Please fix your set of passes.");
		}

		[NotNull]
		private ReadOnlyCollection<NanoPass<string>> local_passes([NotNull] params NanoPass<string>[] passes)
		{
			return passes.ToList()
				.AsReadOnly();
		}

		[NotNull]
		private ReadOnlyCollection<AstStateCondition> requires([NotNull] params string[] condition_names)
		{
			return condition_names.Select(AstStateCondition.named)
				.ToList()
				.AsReadOnly();
		}

		[NotNull]
		private ReadOnlyCollection<AstStateCondition> grants([NotNull] params string[] condition_names)
		{
			return condition_names.Select(AstStateCondition.named)
				.ToList()
				.AsReadOnly();
		}
	}

	public class AppendToName : NanoPass<string>
	{
		[NotNull] private readonly ReadOnlyCollection<AstStateCondition> _causes;
		[NotNull] private readonly string _name_to_append;

		public AppendToName([NotNull] string name_to_append,
			[NotNull] ReadOnlyCollection<AstStateCondition> requires,
			[NotNull] ReadOnlyCollection<AstStateCondition> causes) : base(requires)
		{
			_name_to_append = name_to_append;
			_causes = causes;
		}

		public override string run(string data, Action<AstStateCondition> add_condition)
		{
			var result = string.Format("{0} {1}", data, _name_to_append);
			foreach (var resulting_condition in _causes)
			{
				add_condition(resulting_condition);
			}
			return result;
		}
	}
}
