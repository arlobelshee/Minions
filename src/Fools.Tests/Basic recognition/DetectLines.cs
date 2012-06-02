using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentAssertions;
using Fools.Ast;
using Fools.Compilation;
using Fools.Compilation.Tokenization;
using Fools.Tests.DetectingLines;
using Fools.Tests.Support;
using NUnit.Framework;

namespace Fools.Tests
{
	[TestFixture]
	public class DetectLines
	{
		private static IdentifierToken _identifier(string value)
		{
			return new IdentifierToken(value);
		}

		[Test]
		public void should_find_line_boundaries_and_make_lines()
		{
			The.File(With.Line(3, _identifier("a")), With.Line(2, _identifier("b")))
				.should_contain_lines(
					new Line(3, _identifier("a")), new Line(2, _identifier("b")));
		}
	}
}

namespace Fools.Tests.DetectingLines
{
	public static class BlockAndStatementParsingHelpers
	{
		public static void should_contain_lines(this IEnumerable<Token> token_stream, params INode[] expected)
		{
			var source = new ObserveLists<Token>();
			ReadOnlyListSubject<INode> results = source.detect_lines().Collect();
			source.Send(token_stream);
			results.Should().Equal(expected);
		}
	}
}
