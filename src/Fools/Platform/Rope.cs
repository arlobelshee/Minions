using System;

namespace Fools.Platform
{
	public struct ByteBuffer
	{
		public byte[] data;
		public int length;
	}

	public class Rope
	{
		public BlockSequence enumerate_bytes(int block_size)
		{
			return new BlockSequence(this, block_size);
		}

		public class BlockSequence : Sequence<ByteBuffer>
		{
			private readonly Rope _target;
			private readonly int _block_size;

			public BlockSequence(Rope target, int block_size)
			{
				_target = target;
				_block_size = block_size;
			}

			public Maybe<ByteBuffer> next()
			{
				return Maybe.Empty<ByteBuffer>();
			}
		}
	}
}