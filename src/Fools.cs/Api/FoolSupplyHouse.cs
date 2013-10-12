// FoolSupplyHouse.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Linq;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class FoolSupplyHouse : IDisposable, MissionLocation
	{
		[NotNull] private readonly ActiveCity _active_city;
		[NotNull] private readonly OverlordThrone _overlord_throne;
		[NotNull] private readonly MailRoom _main_drop;

		public FoolSupplyHouse()
		{
			_overlord_throne = new OverlordThrone();
			_main_drop = new MailRoom();
			_active_city = new ActiveCity(_overlord_throne, _main_drop);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_overlord_throne", Justification = "Instance is owned by a different object.")]
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
			_inform_main_drop_about_messages_used(mission);
			_active_city._subscribe_fool_factory_to_respond_to_spawning_messages(mission);
		}

		[Obsolete("Use the active city to do all message announcing. But we've still got some code to move over.")]
		public void announce(MailMessage what_happened)
		{
			_active_city.announce(what_happened);
		}

		private void _inform_main_drop_about_messages_used<TLab>([NotNull] MissionDescription<TLab> mission)
			where TLab : class
		{
			mission.spawning_messages.each(_main_drop.inform_about_message);
			// ReSharper disable PossibleNullReferenceException
			mission.activities.each(activity => _main_drop.inform_about_message(activity.message_type));
			// ReSharper restore PossibleNullReferenceException
		}
	}
}
