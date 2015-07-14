// MissionLocation.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	[UsedForMocking]
	public interface MissionLocation : DeadDrop
	{
		[NotNull]
		CityMap surrounding_city { get; }
		void send_out_fools_to<TLab>([NotNull] MissionDescription<TLab> mission)
			where TLab : class;
		void send_out_main_thread_fools_to<TLab>([NotNull] MissionDescription<TLab> mission)
			where TLab : class;
	}
}
