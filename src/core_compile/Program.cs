// Program.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using Fools.cs.Api;
using Fools.cs.Utilities;

namespace core_compile
{
	[PublicAPI]
	public class Program
	{
		// ReSharper disable InconsistentNaming
		private static int Main([NotNull] string[] args) // ReSharper restore InconsistentNaming
		{
			using (var fools = new FoolSupplyHouse())
			{
				return (int) fools.tell_me_why_I_shouldnt_kill_you(Missions.we_can_do)
					.fine_do_my_bidding(args)
					.while_I_watch_the_fools_dance();
			}
		}
	}
}
