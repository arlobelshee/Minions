using System;
using Fools.Ast;
using Fools.Compilation.Tokenization;

namespace Fools.Compilation
{
	public class UnderstandFools : IObservable<INode>
	{
		private readonly IObservable<INode> _parser;
		private readonly FoolsTokenStream _pipelineStart;

		public UnderstandFools(string fool)
		{
			_pipelineStart = new FoolsTokenStream(fool);
			_parser = _pipelineStart
				.DetectLines()
				.RecognizeBlocksAndStatements();
		}

		public IDisposable Subscribe(IObserver<INode> observer)
		{
			return _parser.Subscribe(observer);
		}

		public void Go()
		{
			_pipelineStart.Read();
		}
	}
}
