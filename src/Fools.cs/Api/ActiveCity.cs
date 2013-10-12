// ActiveCity.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Linq;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class ActiveCity : IDisposable
	{
		[NotNull] private readonly Fool<MailRoom> _postal_carrier;
		[NotNull] private readonly OverlordThrone _overlord_throne;
		[NotNull] private readonly MailRoom _main_drop;
		[NotNull] private readonly FoolFactory _fool_factory;

		public ActiveCity([NotNull] OverlordThrone overlord_throne, [NotNull] MailRoom main_drop)
		{
			_overlord_throne = overlord_throne;
			_main_drop = main_drop;
			_fool_factory = FoolFactory.using_background_threads();
			_postal_carrier = _fool_factory.create_fool(_main_drop);
		}

		public void Dispose()
		{
			_fool_factory.Dispose();
			_overlord_throne.Dispose();
		}

		public void announce([NotNull] MailMessage what_happened)
		{
			_postal_carrier.do_work(mail_room => // ReSharper disable PossibleNullReferenceException
				mail_room
					// ReSharper restore PossibleNullReferenceException
					.announce(what_happened),
				FoolFactory.noop);
		}

		[NotNull]
		public OverlordThrone make_the_fools_dance([NotNull] string[] args)
		{
			announce(new DoMyBidding(args));
			return _overlord_throne;
		}

		public void _subscribe_fool_factory_to_respond_to_spawning_messages<TLab>([NotNull] MissionDescription<TLab> mission)
			where TLab : class
		{
			mission.spawning_messages.each(message_type => _main_drop
				// ReSharper disable AssignNullToNotNullAttribute
				.subscribe(message_type, (message, done) => _spawn_fool(mission, done, message)));
			// ReSharper restore AssignNullToNotNullAttribute
		}

		public void announce_and_notify_when_done([NotNull] MailMessage what_happened, Action when_done)
		{
			_postal_carrier.do_work(mail_room => // ReSharper disable PossibleNullReferenceException
				mail_room
					// ReSharper restore PossibleNullReferenceException
					.announce_and_notify_when_done(what_happened, when_done),
				FoolFactory.noop);
		}

		private void _spawn_fool<TLab>([NotNull] MissionDescription<TLab> mission,
			[NotNull] Action done_creating_fool,
			[NotNull] MailMessage constructor_message) where TLab : class
		{
			var fool = _fool_factory.create_fool(mission.make_lab());
			_execute_fool_ctor(mission, done_creating_fool, constructor_message, fool);
			_subscribe_handlers_for_rest_of_mission(mission, fool);
		}

		private static void _execute_fool_ctor<TLab>([NotNull] MissionDescription<TLab> mission,
			[NotNull] Action done_creating_fool,
			[NotNull] MailMessage constructor_message,
			[NotNull] Fool<TLab> fool) where TLab : class
		{
			var mission_activity = mission.activity_for(constructor_message);
			if (mission_activity == null) done_creating_fool();
			else // ReSharper disable AssignNullToNotNullAttribute
				fool.do_work(lab => mission_activity.execute(lab, constructor_message), done_creating_fool);
			// ReSharper restore AssignNullToNotNullAttribute
		}

		private void _subscribe_handlers_for_rest_of_mission<TLab>([NotNull] MissionDescription<TLab> mission,
			[NotNull] Fool<TLab> fool) where TLab : class
		{
			_postal_carrier.upon_completion_of_this_task(mail_room => mission.activities.each(activity => {
				if (activity == null) return;
				// ReSharper disable PossibleNullReferenceException
				mail_room.subscribe(activity.message_type,
					// ReSharper restore PossibleNullReferenceException
					// ReSharper disable AssignNullToNotNullAttribute
					(message, done_handling_message) => fool.do_work(lab => activity.execute(lab, message), done_handling_message));
				// ReSharper restore AssignNullToNotNullAttribute
			}));
		}
	}
}
