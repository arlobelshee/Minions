// MailRoom.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Linq;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class MailRoom
	{
		[NotNull] private readonly Fool<MailIndex> _postal_carrier;

		public MailRoom([NotNull] MailIndex delivery_routes, [NotNull] FoolFactory fool_factory)
		{
			_postal_carrier = fool_factory.create_fool(delivery_routes);
		}

		public void announce([NotNull] MailMessage what_happened)
		{
			_postal_carrier.do_work(delivery_routes => // ReSharper disable PossibleNullReferenceException
				delivery_routes
					// ReSharper restore PossibleNullReferenceException
					.announce(what_happened),
				FoolFactory.noop);
		}

		public void announce_and_notify_when_done([NotNull] MailMessage what_happened, [NotNull] Action when_done)
		{
			_postal_carrier.do_work(delivery_routes => // ReSharper disable PossibleNullReferenceException
				delivery_routes
					// ReSharper restore PossibleNullReferenceException
					.announce_and_notify_when_done(what_happened, when_done),
				FoolFactory.noop);
		}

		public void define_mission<TLab>([NotNull] FoolFactory fool_factory, [NotNull] MissionDescription<TLab> mission)
			where TLab : class
		{
			_postal_carrier.do_work(delivery_routes => mission.spawning_messages.each(message_type =>
				// ReSharper disable PossibleNullReferenceException
				delivery_routes
					// ReSharper restore PossibleNullReferenceException
					// ReSharper disable AssignNullToNotNullAttribute
					.subscribe(message_type, (message, done) => _spawn_fool(fool_factory, mission, done, message))),
				// ReSharper restore AssignNullToNotNullAttribute
				FoolFactory.noop);
		}

		private void _spawn_fool<TLab>([NotNull] FoolFactory fool_factory,
			[NotNull] MissionDescription<TLab> mission,
			[NotNull] Action done_creating_fool,
			[NotNull] MailMessage constructor_message) where TLab : class
		{
			var fool = fool_factory.create_fool(mission.make_lab());
			_subscribe_handlers_for_rest_of_mission(mission, fool);
			_execute_fool_ctor(mission, done_creating_fool, constructor_message, fool);
		}

		private void _subscribe_handlers_for_rest_of_mission<TLab>([NotNull] MissionDescription<TLab> mission,
			[NotNull] Fool<TLab> fool) where TLab : class
		{
			_postal_carrier.upon_completion_of_this_task(delivery_routes => mission.activities.each(activity => {
				if (activity == null) return;
				// ReSharper disable PossibleNullReferenceException
				delivery_routes.subscribe(activity.message_type,
					// ReSharper restore PossibleNullReferenceException
					// ReSharper disable AssignNullToNotNullAttribute
					(message, done_handling_message) => fool.do_work(lab => activity.execute(lab, message), done_handling_message));
				// ReSharper restore AssignNullToNotNullAttribute
			}));
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
	}
}
