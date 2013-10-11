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
		[NotNull] private readonly object _guard = new object();
		[CanBeNull] private ManualResetEventSlim _have_met_intermediate_goal;
		private int _intermediate_goal;
		private int _number_of_callers_remaining;

		private NonBlockingBarrier(int number_of_callers_to_expect, [NotNull] Action when_satisfied)
		{
			_number_of_callers_remaining = number_of_callers_to_expect;
			_announce_is_met = when_satisfied;
			_have_met_intermediate_goal = null;
			_intermediate_goal = 0;
		}

		public int remaining_count { get { return _number_of_callers_remaining; } }

		[NotNull]
		public static NonBlockingBarrier starting_at(int number_of_callers_to_expect, [NotNull] Action when_satisfied)
		{
			return new NonBlockingBarrier(number_of_callers_to_expect, when_satisfied);
		}

		public void notify()
		{
			var do_announcement = false;
			lock (_guard)
			{
				if (_number_of_callers_remaining == 0) return;
				--_number_of_callers_remaining;
				if (_number_of_callers_remaining == 0) do_announcement = true;
				if (_have_met_intermediate_goal != null && _number_of_callers_remaining == _intermediate_goal) _have_met_intermediate_goal.Set();
			}
			if (do_announcement) _announce_is_met();
		}

		public bool wait_until_count_reaches(int number_remaining, TimeSpan wait_duration)
		{
			using (var have_met_intermediate_goal = new ManualResetEventSlim(false))
			{
				lock (_guard)
				{
					if (number_remaining >= _number_of_callers_remaining) return true;
					_intermediate_goal = number_remaining;
					_have_met_intermediate_goal = have_met_intermediate_goal;
				}
				return have_met_intermediate_goal.Wait(wait_duration);
			}
		}
	}
}
