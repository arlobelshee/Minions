// FoolSupplyHouse.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Diagnostics.CodeAnalysis;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class FoolSupplyHouse : IDisposable, MissionLocation
	{
		[NotNull] private readonly ActiveCity _active_city;
		[NotNull] private readonly OverlordThrone _overlord_throne;

		public FoolSupplyHouse()
		{
			_overlord_throne = new OverlordThrone();
			var city_map = new CityMap();
			var city_hall = city_map.public_location(PublicBuildings.APPLICATION)
				.will_pass_message<DoMyBidding>()
				.will_pass_message<AppQuit>();
			_active_city = new ActiveCity(_overlord_throne, city_hall);
		}

		[SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_overlord_throne",
			Justification = "Instance is owned by a different object.")]
		public void Dispose()
		{
			_active_city.Dispose();
		}

		[NotNull]
		public ActiveCity build_me_a_city([NotNull] Action<MissionLocation> missions_we_can_do)
		{
			missions_we_can_do(this);
			_overlord_throne.show_him_what_you_do(this);
			return _active_city;
		}

		public void send_out_fools_to<TLab>(MissionDescription<TLab> mission) where TLab : class
		{
			_active_city.define_mission(mission);
		}

		[Obsolete("Use the active city to do all message announcing. But we've still got some code to move over.")]
		public void announce(MailMessage what_happened)
		{
			_active_city.city_hall.announce(what_happened);
		}
	}
}
