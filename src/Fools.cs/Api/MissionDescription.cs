// MissionDescription.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	/// <summary>
	///    Describes one mission that we can send fools on.
	///    Missions tell fools how to respond to messages. Each mission can have 0:many
	///    fools on it at a time. All of them will respond to a received message. Send different
	///    fools to different mission locations if you wish to talk to them individually.
	///    The mission also tells mission control when to spawn a new fool. The description
	///    includes one or more messages that cause a fool to be spawned. Each time one of
	///    these messages arrive, mission control will spawn one new fool. Mission control
	///    will send this fool on this mission at a particular mission location. The location is
	///    specified as part of the spawn data associated with the message received.
	///    Use the NewMission factory class to build mission instances.
	/// </summary>
	public class MissionDescription<TLab> where TLab : class
	{
		[NotNull] private readonly Func<TLab> _lab_constructor;
		[NotNull] private readonly List<Type> _spawning_messages = new List<Type>();

		[NotNull] private readonly Dictionary<Type, MissionActivity<TLab>> _responses =
			new Dictionary<Type, MissionActivity<TLab>>();

		internal MissionDescription([NotNull] Func<TLab> lab_constructor)
		{
			_lab_constructor = lab_constructor;
		}

		[NotNull]
		public IEnumerable<Type> spawning_messages { get { return _spawning_messages; } }

		[NotNull]
		public IEnumerable<MissionActivity<TLab>> activities { get { return _responses.Values; } }

		[CanBeNull]
		public MissionActivity<TLab> activity_for([NotNull] MailMessage message)
		{
			MissionActivity<TLab> activity;
			var found = _responses.TryGetValue(message.GetType(), out activity);
			return found ? activity : null;
		}

		[NotNull]
		public MissionSpawnOptions<TStartMessage> send_new_fool_when<TStartMessage>() where TStartMessage : MailMessage
		{
			_spawning_messages.Add(typeof (TStartMessage));
			return new MissionSpawnOptions<TStartMessage>(this);
		}

		[NotNull]
		public MissionDescription<TLab> fools_shall_do<TMessage>([NotNull] Action<TLab, TMessage> message_response)
			where TMessage : MailMessage
		{
			_responses[typeof (TMessage)] = new MissionActivityTypeSpecific<TLab, TMessage>(message_response);
			return this;
		}

		public class MissionSpawnOptions<TMessage> where TMessage : MailMessage
		{
			[NotNull] private readonly MissionDescription<TLab> _mission_to_update;

			public MissionSpawnOptions([NotNull] MissionDescription<TLab> mission_to_update)
			{
				_mission_to_update = mission_to_update;
			}

			[NotNull]
			public MissionDescription<TLab> and_have_it([NotNull] Action<TLab, TMessage> message_response)
			{
				_mission_to_update.fools_shall_do(message_response);
				return _mission_to_update;
			}
		}

		[NotNull]
		public TLab make_lab()
		{
			// ReSharper disable AssignNullToNotNullAttribute
			return _lab_constructor();
			// ReSharper restore AssignNullToNotNullAttribute
		}
	}
}
