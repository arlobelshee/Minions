using System;
using Fools.Ast;
using Fools.Compilation.Recognizing;
using Fools.Compilation.Tokenization;
using System.Reactive.Linq;
using MetaSharp.Transformation;
using INode = Fools.Ast.INode;

namespace Fools.Compilation
{
	public static class Stage
	{
		public static IObservable<INode> detect_lines(this IObservable<Token> source)
		{
			return new LineDetector().subscribed_to(source);
		}

		public static IObservable<INode> recognize_blocks_and_statements(this IObservable<INode> source)
		{
			return new BlockFinder().subscribed_to(source);
		}

		public static IObservable<INode> understand(this IObservable<INode> source)
		{
			return source.Select(understand_one_tree);
		}

		public static INode understand_one_tree(INode src)
		{
			if (src is Block)
				return understand_one_block(src as Block);
			return understand_one_statement(src as UnrecognizedStatement);
		}

		public static Block understand_one_block(Block source)
		{
			return source;
		}

		public static IStatement understand_one_statement(UnrecognizedStatement source)
		{
			var context = new PatternContext();
			return (IStatement) new BasicFools().Transform(context, new NodeStream(source.contents));
			//return source;
		}

		public static IObservable<TDest> subscribed_to<TSource, TDest>(
			this ITransformation<TSource, TDest> dest, IObservable<TSource> source)
		{
			source.Subscribe(dest);
			return dest;
		}
	}
}
