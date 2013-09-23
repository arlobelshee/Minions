﻿// FoolSupplyHouse.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class FoolSupplyHouse : IDisposable, MissionLocation
	{
		[NotNull] private readonly TaskFactory _task_factory;
		[NotNull] private readonly CancellationTokenSource _cancellation;
		[NotNull] private readonly Fool<MailRoom> _postal_carrier;
		[NotNull] private readonly OverlordThrone _overlord_throne;
		[NotNull] private readonly MailRoom _main_drop;

		public FoolSupplyHouse()
		{
			_overlord_throne = new OverlordThrone();
			_cancellation = new CancellationTokenSource();
			_task_factory = new TaskFactory(_cancellation.Token,
				TaskCreationOptions.PreferFairness,
				TaskContinuationOptions.ExecuteSynchronously,
				TaskScheduler.Default);
			_main_drop = new MailRoom();
			_postal_carrier = new Fool<MailRoom>(_create_starting_task(), _main_drop);

			_overlord_throne.show_him_what_you_do(this);
		}

		public void Dispose()
		{
			_cancellation.Cancel();
			_cancellation.Dispose();
			_overlord_throne.Dispose();
		}

		[NotNull]
		public FoolSupplyHouse tell_me_why_I_shouldnt_kill_you([NotNull] Action<MissionLocation> missions_we_can_do)
		{
			missions_we_can_do(this);
			return this;
		}

		public void send_out_fools_to<TLab>(MissionDescription<TLab> mission) where TLab : class
		{
			_inform_main_drop_about_messages_used(mission);
			_subscribe_fool_factory_to_respond_to_spawning_messages(mission);
		}

		private void _subscribe_fool_factory_to_respond_to_spawning_messages<TLab>([NotNull] MissionDescription<TLab> mission)
			where TLab : class
		{
			mission.spawning_messages.each(message_type => _main_drop
				// ReSharper disable AssignNullToNotNullAttribute
				.subscribe(message_type, (message, done) => _spawn_fool(mission, done, message)));
			// ReSharper restore AssignNullToNotNullAttribute
		}

		private void _inform_main_drop_about_messages_used<TLab>([NotNull] MissionDescription<TLab> mission)
			where TLab : class
		{
			mission.spawning_messages.each(_main_drop.inform_about_message);
			// ReSharper disable PossibleNullReferenceException
			mission.activities.each(activity => _main_drop.inform_about_message(activity.message_type));
			// ReSharper restore PossibleNullReferenceException
		}

		[NotNull]
		public OverlordThrone fine_do_my_bidding([NotNull] string[] args)
		{
			announce(new DoMyBidding(args));
			return _overlord_throne;
		}

		public void announce(MailMessage what_happened)
		{
			_postal_carrier.do_work(mail_room => // ReSharper disable PossibleNullReferenceException
				mail_room
					// ReSharper restore PossibleNullReferenceException
					.announce(what_happened),
				_noop);
		}

		public bool announce_and_wait([NotNull] MailMessage what_happened, TimeSpan wait_duration)
		{
			bool? result = null;
			return _postal_carrier.do_work_and_wait(mail_room => {
				result = // ReSharper disable PossibleNullReferenceException
					mail_room
						// ReSharper restore PossibleNullReferenceException
						.announce_and_wait(what_happened, wait_duration);
			},
				wait_duration) && result.GetValueOrDefault(false);
		}

		[NotNull]
		private Task _create_starting_task()
		{
			return _task_factory.StartNew(_noop);
		}

		private void _spawn_fool<TLab>([NotNull] MissionDescription<TLab> mission,
			[NotNull] Action done_creating_fool,
			[NotNull] MailMessage constructor_message) where TLab : class
		{
			var fool = new Fool<TLab>(_create_starting_task(), mission.make_lab());
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
			else fool.process_message(mission_activity, constructor_message, done_creating_fool);
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
					(message, done_handling_message) => fool.process_message(activity, message, done_handling_message));
				// ReSharper restore AssignNullToNotNullAttribute
			}));
		}

		private static void _noop() {}
	}
}
