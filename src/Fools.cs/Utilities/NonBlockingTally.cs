// NonBlockingBarrier.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Threading;

namespace Fools.cs.Utilities
{
	public class NonBlockingTally
	{
		private int _num_already_seen;
		[CanBeNull]
		private ManualResetEventSlim _have_met_intermediate_goal;
		private int _intermediate_goal;

		private NonBlockingTally(int num_already_seen)
		{
			_num_already_seen = num_already_seen;
		}

		[NotNull]
		public static NonBlockingTally starting_at(int num_already_seen)
		{
			return new NonBlockingTally(num_already_seen);
		}

		public void notify()
		{
			var num_seen = Interlocked.Increment(ref _num_already_seen);
			var intermediate_goal = _have_met_intermediate_goal;
			if (intermediate_goal != null && num_seen >= _intermediate_goal) intermediate_goal.Set();
		}

		public bool wait_until_count_reaches(int num_seen, TimeSpan wait_duration)
		{
			if (num_seen <= _num_already_seen) return true;
			using (var have_met_intermediate_goal = new ManualResetEventSlim(false))
			{
				_intermediate_goal = num_seen;
				Interlocked.Exchange(ref _have_met_intermediate_goal, have_met_intermediate_goal);
				if (num_seen <= _num_already_seen)
				{
					Interlocked.Exchange(ref _have_met_intermediate_goal, null);
					return true;
				}
				var wait_result = have_met_intermediate_goal.Wait(wait_duration);
				Interlocked.Exchange(ref _have_met_intermediate_goal, null);
				return wait_result;
			}
		}
	}
}
