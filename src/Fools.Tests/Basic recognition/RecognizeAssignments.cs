using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentAssertions;
using Fools.Ast;
using Fools.Compilation;
using Fools.Compilation.Tokenization;
using Fools.Tests.Support;
using NUnit.Framework;

namespace Fools.Tests.Basic_recognition
{
	[TestFixture]
	public class RecognizeAssignments
	{
		[Test]
		public void should_detect_simple_assignment_from_variable()
		{
			UnrecognizedStatement source = With.Statement(Identifier("variable"), Identifier("="), Identifier("value"));
			FindAssignments(source).Should().Be(Assigment(Identifier("value"), Identifier("variable")));
		}

		[Test]
		public void should_leave_non_assignment_unchanged()
		{
			UnrecognizedStatement source = With.Statement(Identifier("a.b"), Identifier(","), Identifier("c.d"));
			FindAssignments(source).Should().Be(source);
		}

		private IStatement FindAssignments(UnrecognizedStatement source)
		{
			return Stage.understand_one_statement(source);
		}

		private AssignmentStatement Assigment(IdentifierToken from, IdentifierToken to)
		{
			return new AssignmentStatement
			       {
			       	value = new VariableReferenceExpression(@from),
			       	variable = new VariableReferenceExpression(to)
			       };
		}

		private static IdentifierToken Identifier(string value)
		{
			return new IdentifierToken(value);
		}
	}
}
