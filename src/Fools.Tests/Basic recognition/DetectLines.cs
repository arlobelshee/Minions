﻿using System.Collections.Generic;
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
		private static IdentifierToken Identifier(string value)
		{
			return new IdentifierToken(value);
		}

		[Test]
		public void ShouldFindLineBoundariesAndMakeLines()
		{
			The.File(With.Line(3, Identifier("a")), With.Line(2, Identifier("b")))
				.ShouldContainLines(
					new Line(3, Identifier("a")), new Line(2, Identifier("b")));
		}
	}
}

namespace Fools.Tests.DetectingLines
{
	public static class BlockAndStatementParsingHelpers
	{
		public static void ShouldContainLines(this IEnumerable<Token> tokenStream, params INode[] expected)
		{
			var source = new ObserveLists<Token>();
			ReadOnlyListSubject<INode> results = source.DetectLines().Collect();
			source.Send(tokenStream);
			results.Should().Equal(expected);
		}
	}
}
