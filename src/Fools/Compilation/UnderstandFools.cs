using System;
using Fools.Ast;
using Fools.Compilation.Tokenization;

namespace Fools.Compilation
{
	public class UnderstandFools : IObservable<INode>
	{
		private readonly IObservable<INode> _parser;
		private readonly FoolsTokenStream _pipeline_start;

		public UnderstandFools(string fool)
		{
			_pipeline_start = new FoolsTokenStream(fool);
			_parser = _pipeline_start
				.detect_lines()
				.recognize_blocks_and_statements()
				.understand();
		}

		public IDisposable Subscribe(IObserver<INode> observer)
		{
			return _parser.Subscribe(observer);
		}

		public void go()
		{
			_pipeline_start.Read();
		}
	}
}
