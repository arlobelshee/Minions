// NonBlockingBarrier.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Threading;

namespace Fools.cs.Utilities
{
	public class NonBlockingBarrier
	{
		[NotNull] private readonly Action _announce_is_met;
		[CanBeNull] private ManualResetEventSlim _have_met_intermediate_goal;
		private int _intermediate_goal;
		private int _number_of_callers_remaining;

		private NonBlockingBarrier(int number_of_callers_to_expect, [NotNull] Action when_satisfied)
		{
			_number_of_callers_remaining = number_of_callers_to_expect;
			_announce_is_met = when_satisfied;
			_have_met_intermediate_goal = null;
			_intermediate_goal = -1;
		}

		public int remaining_count { get { return _number_of_callers_remaining; } }

		[NotNull]
		public static NonBlockingBarrier starting_at(int number_of_callers_to_expect, [CanBeNull] Action when_satisfied = null)
		{
			return new NonBlockingBarrier(number_of_callers_to_expect, when_satisfied ?? (() => { }));
		}

		public void notify()
		{
			if (_number_of_callers_remaining == 0) return;
			var num_now_remaining = Interlocked.Decrement(ref _number_of_callers_remaining);
			if (num_now_remaining == 0) _announce_is_met();
			var intermediate_goal = _have_met_intermediate_goal;
			if (intermediate_goal != null && num_now_remaining == _intermediate_goal) intermediate_goal.Set();
		}

		public bool wait_until_count_reaches(int number_remaining, TimeSpan wait_duration)
		{
			if (number_remaining >= _number_of_callers_remaining) return true;
			using (var have_met_intermediate_goal = new ManualResetEventSlim(false))
			{
				_intermediate_goal = number_remaining;
				Interlocked.Exchange(ref _have_met_intermediate_goal, have_met_intermediate_goal);
				if (number_remaining >= _number_of_callers_remaining)
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
