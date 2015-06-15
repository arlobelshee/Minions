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
		[NotNull] private readonly FoolFactory _fool_factory;
		[NotNull] private readonly MailRoom _city_hall;

		public ActiveCity([NotNull] OverlordThrone overlord_throne, [NotNull] MailIndex main_drop)
		{
			_overlord_throne = overlord_throne;
			_fool_factory = FoolFactory.using_background_threads();
			_city_hall = new MailRoom(main_drop, _fool_factory);
		}

		[NotNull]
		public MailRoom city_hall { get { return _city_hall; } }

		public void Dispose()
		{
			_fool_factory.Dispose();
			_overlord_throne.Dispose();
		}

		[NotNull]
		public OverlordThrone make_the_fools_dance([NotNull] string[] args)
		{
			_city_hall.announce(new DoMyBidding(args));
			return _overlord_throne;
		}

		public void define_mission<TLab>([NotNull] MissionDescription<TLab> mission) where TLab : class
		{
			_city_hall.define_mission(_fool_factory, mission);
		}
	}
}
