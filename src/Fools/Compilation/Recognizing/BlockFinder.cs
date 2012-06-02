using System.Collections.Generic;
using System.Linq;
using Fools.Ast;
using Fools.Compilation.Tokenization;

namespace Fools.Compilation.Recognizing
{
	public class BlockFinder : Transformation<INode, INode>
	{
		private static readonly IdentifierToken _COLON = new IdentifierToken(":");
		private readonly Stack<Block> _current_blocks = new Stack<Block>();

		public override void OnNext(INode value)
		{
			_handle_line((Line) value);
		}

		public override void OnCompleted()
		{
			_end_block_if_needed(0);
			base.OnCompleted();
		}

		private void _handle_line(Line value)
		{
			_end_block_if_needed(value.IndentationLevel);
			if(value.Contents.Last() == _COLON)
				_add_block(new Block(value.Contents.Take(value.Contents.Count - 1)));
			else
				_add_statement_to_current_block(new UnrecognizedStatement(value.Contents));
		}

		private void _add_statement_to_current_block(IStatement statement)
		{
			if(_current_blocks.Count == 0)
				send_next(statement);
			else
				_current_blocks.Peek().AddStatement(statement);
		}

		private void _end_block_if_needed(int new_indentation_level)
		{
			if(new_indentation_level >= _current_blocks.Count || _current_blocks.Count == 0) return;
			while(_current_blocks.Count > 1) _current_blocks.Pop();
			send_next(_current_blocks.Pop());
		}

		private void _add_block(Block block)
		{
			if(_current_blocks.Count > 0) _current_blocks.Peek().AddStatement(block);
			_current_blocks.Push(block);
		}
	}
}
