using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentAssertions;
using Fools.Ast;
using Fools.Compilation;
using Fools.Compilation.Tokenization;
using Fools.Tests.BlocksAndStatements;
using Fools.Tests.Support;
using NUnit.Framework;

namespace Fools.Tests
{
	[TestFixture]
	public class BlockAndStatementParsing
	{
		[Test]
		public void should_detect_a_simple_statement()
		{
			Lines(
				Line(0, Identifier("some"), Identifier("statement"))
				)
				.should_be_recognized_as(
					With.Statement(Identifier("some"), Identifier("statement")));
		}

		[Test]
		public void should_detect_a_non_nested_block()
		{
			Lines(
				Line(0, Identifier("some"), Identifier("block.header"), Identifier(":")),
				Line(1, Identifier("do.something")),
				Line(1, Identifier("do.something.else"))
				)
				.should_be_recognized_as(
					new Block(
						With.Tokens(Identifier("some"), Identifier("block.header")),
						With.Statement(Identifier("do.something")),
						With.Statement(Identifier("do.something.else"))));
		}

		[Test]
		public void should_detect_sequential_non_nested_blocks()
		{
			Lines(
				Line(0, Identifier("some"), Identifier("block.header"), Identifier(":")),
				Line(1, Identifier("pass")),
				Line(0, Identifier("another"), Identifier("block.header"), Identifier(":")),
				Line(1, Identifier("pass"))
				)
				.should_be_recognized_as(
					new Block(
						With.Tokens(Identifier("some"), Identifier("block.header")),
						With.Statement(Identifier("pass"))),
					new Block(
						With.Tokens(Identifier("another"), Identifier("block.header")),
						With.Statement(Identifier("pass"))));
		}

		[Test]
		public void should_detect_nested_blocks()
		{
			Lines(
				Line(0, Identifier("some"), Identifier("block.header"), Identifier(":")),
				Line(1, Identifier("another"), Identifier("block.header"), Identifier(":")),
				Line(2, Identifier("pass"))
				)
				.should_be_recognized_as(
					new Block(
						With.Tokens(Identifier("some"), Identifier("block.header")),
						new Block(
							With.Tokens(Identifier("another"), Identifier("block.header")),
							With.Statement(Identifier("pass")))));
		}

		private static IEnumerable<Line> Lines(params Line[] lines)
		{
			return lines;
		}

		private static Line Line(int indentation_level, params Token[] contents)
		{
			return new Line(indentation_level, contents);
		}

		private static IdentifierToken Identifier(string value)
		{
			return new IdentifierToken(value);
		}
	}
}

namespace Fools.Tests.BlocksAndStatements
{
	public static class BlockAndStatementParsingHelpers
	{
		public static void should_be_recognized_as(this IEnumerable<Line> token_stream, params INode[] expected)
		{
			var source = new ObserveLists<INode>();
			ReadOnlyListSubject<INode> results = source.recognize_blocks_and_statements().Collect();
			source.Send(token_stream);
			results.Should().Equal(expected);
		}
	}
}
