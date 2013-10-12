// WaitableCounter.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Threading;

namespace Fools.cs.Utilities
{
	public abstract class WaitableCounter
	{
		[NotNull]
		public static WaitableCounter starting_at(int initial_value, [NotNull] Action when_done)
		{
			return new NonBlockingCountingImpl(initial_value, when_done);
		}

		[NotNull]
		public static WaitableCounter non_counting()
		{
			return new NonCountingCounter();
		}

		public abstract void begin();
		public abstract void done();

		private class NonBlockingCountingImpl : WaitableCounter
		{
			private int _count;
			[NotNull] private readonly Action _when_done;

			public NonBlockingCountingImpl(int initial_value, [NotNull] Action when_done)
			{
				_count = initial_value;
				_when_done = when_done;
			}

			public override void begin()
			{
				Interlocked.Increment(ref _count);
			}

			public override void done()
			{
				var current_count = Interlocked.Decrement(ref _count);
				if (current_count == 0) _when_done();
			}
		}

		private class NonCountingCounter : WaitableCounter
		{
			public override void begin() {}

			public override void done() {}
		}
	}
}
