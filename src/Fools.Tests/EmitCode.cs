using System.Collections.Generic;
using System.Reflection.Emit;
using ApprovalTests;
using ApprovalTests.Reporters;
using Fools.Ast;
using Fools.Compilation.Tokenization;
using NUnit.Framework;
using MethodBuilder = Fools.Compilation.Generation.MethodBuilder;

namespace Fools.Tests
{
	[TestFixture, Ignore, UseReporter(typeof(QuietReporter))]
	public class EmitCode
	{
		private static void _approve_result_of_execution(INode node)
		{
			Dictionary<string, object> frame = _evaluate_and_return_frame(node);
			Approvals.Approve(frame, kv => string.Format("{0} = {1} [{2}]", kv.Key, kv.Value, kv.Value.GetType()));
		}

		private static Dictionary<string, object> _evaluate_and_return_frame(INode node)
		{
			DynamicMethod method = _make_an_assignment(node);
			var frame = new Dictionary<string, object>();
			method.Invoke(
				null,
				new[]
				{
					frame
				});
			return frame;
		}

		private static DynamicMethod _make_an_assignment(INode node)
		{
			var factory = new MethodBuilder();
			factory.AddAssignmentStatement(node as AssignmentStatement);
			return factory.ToCode();
		}

		[Test]
		public void assignment()
		{
			_approve_result_of_execution(
				new AssignmentStatement
				{
					variable = new VariableReferenceExpression(new IdentifierToken("a")),
					value = new NumberLiteral(3)
				});
		}
	}
}
