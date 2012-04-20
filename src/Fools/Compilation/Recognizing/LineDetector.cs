using System.Collections.Generic;
using System.Linq;
using Fools.Ast;
using Fools.Compilation.Tokenization;

namespace Fools.Compilation.Recognizing
{
	public class LineDetector : Transformation<Token, INode>
	{
		private readonly List<Token> _tokensSeen = new List<Token>();

		protected Token[] LineContents { get { return _tokensSeen.Skip(1).ToArray(); } }

		private int Indent { get { return ((IndentationToken) _tokensSeen[0]).IndentationLevel; } }

		public override void OnNext(Token value)
		{
			if(value is EndOfStatementToken)
			{
				SendNext(new Line(Indent, LineContents));
				_tokensSeen.Clear();
			}
			else
			{
				_tokensSeen.Add(value);
			}
		}
	}
}
