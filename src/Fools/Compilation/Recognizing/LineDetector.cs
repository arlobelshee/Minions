using System.Collections.Generic;
using System.Linq;
using Fools.Ast;
using Fools.Compilation.Tokenization;

namespace Fools.Compilation.Recognizing
{
	public class LineDetector : Transformation<Token, INode>
	{
		private readonly List<Token> _tokens_seen = new List<Token>();

		protected Token[] line_contents { get { return _tokens_seen.Skip(1).ToArray(); } }

		private int _indent { get { return ((IndentationToken) _tokens_seen[0]).IndentationLevel; } }

		public override void OnNext(Token value)
		{
			if(value is EndOfStatementToken)
			{
				send_next(new Line(_indent, line_contents));
				_tokens_seen.Clear();
			}
			else
			{
				_tokens_seen.Add(value);
			}
		}
	}
}
