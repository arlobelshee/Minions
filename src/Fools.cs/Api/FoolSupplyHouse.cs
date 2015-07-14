// FoolSupplyHouse.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Diagnostics.CodeAnalysis;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class FoolSupplyHouse : IDisposable
	{
		[NotNull] private readonly ActiveCity _home_town;
		[NotNull] private readonly OverlordThrone _overlord_throne;

		public FoolSupplyHouse([NotNull] FoolFactory main_thread_factory)
		{
			_overlord_throne = new OverlordThrone();
			_home_town = new ActiveCity(_overlord_throne, main_thread_factory);
		}

		[SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_overlord_throne",
			Justification = "Instance is owned by a different object.")]
		public void Dispose()
		{
			_home_town.Dispose();
		}

		[NotNull]
		public ActiveCity build_me_a_city([NotNull] Action<MissionLocation> missions_we_can_do)
		{
			missions_we_can_do(_home_town.city_hall);
			_overlord_throne.show_him_what_you_do(_home_town.city_hall);
			return _home_town;
		}

		[Obsolete("Use the active city to do all message announcing. But we've still got some code to move over.")]
		public void announce([NotNull] MailMessage what_happened)
		{
			_home_town.city_hall.announce(what_happened);
		}
	}
}
