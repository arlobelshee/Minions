using FluentAssertions;
using Fools.Compilation.Generation;
using NUnit.Framework;

namespace Fools.Tests
{
	[TestFixture, Ignore]
	public class CompilerAcceptanceTests
	{
		private CodeUnit Compile(string fool)
		{
			//INode node = new RecognizeBlocksAndStatements().Parse(fool);
			//return new CodeUnit(node);
			return null;
		}

		[Test]
		public void PrintThisOut()
		{
//         var fool = @"
//def foo():
//	p";
			const string fool = "a=3";
			var interpreter = new Interpreter();
			interpreter.evalute(Compile(fool));
			interpreter.Variables.Should().Contain("a", 3);
		}
	}
}
