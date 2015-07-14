// ActiveCity.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class ActiveCity : IDisposable
	{
		[NotNull] private readonly OverlordThrone _overlord_throne;
		[NotNull] private readonly MailRoom _city_hall;
		[NotNull] private readonly CityMap _map;

		public ActiveCity([NotNull] OverlordThrone overlord_throne, [NotNull] FoolFactory main_thread_factory)
		{
			_overlord_throne = overlord_throne;
			_map = new CityMap(main_thread_factory);
			var city_hall_location = _map.public_location(PublicBuildings.APPLICATION)
				.will_pass_message<DoMyBidding>()
				.will_pass_message<AppQuit>();
			_city_hall = city_hall_location.create_dead_drop(_map.regular_fools);
		}

		[NotNull]
		public MailRoom city_hall { get { return _city_hall; } }

		[NotNull]
		public CityMap map { get { return _map; } }

		public void Dispose()
		{
			_map.Dispose();
			_overlord_throne.Dispose();
		}

		[NotNull]
		public OverlordThrone make_the_fools_dance([NotNull] string[] args)
		{
			_city_hall.announce(new DoMyBidding(args));
			return _overlord_throne;
		}
	}
}
