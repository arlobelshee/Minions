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
			using (var fools = new FoolSupplyHouse(FoolFactory.using_background_threads()))
			{
				return (int) fools.build_me_a_city(For.my_amusement)
					.make_the_fools_dance(args)
					.while_I_wait_for_the_world_to_end();
			}
		}
	}
}
