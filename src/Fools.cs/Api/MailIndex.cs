// MailRoom.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class MailIndex : DeadDrop
	{
		[NotNull]
		public string name { get; private set; }

		public delegate void MessageHandler(MailMessage message, Action done);

		[NotNull] private readonly NonNullDictionary<string, NonNullList<MessageHandler>> _listeners =
			new NonNullDictionary<string, NonNullList<MessageHandler>>();

		public MailIndex([NotNull] IEnumerable<Type> messages_supported, [NotNull] string name)
		{
			this.name = name;
			messages_supported.each(inform_about_message);
		}

		public ReadOnlyCollection<string> allowed_messages
		{
			get
			{
				return _listeners.Keys.ToList()
					.AsReadOnly();
			}
		}

		public void inform_about_message([NotNull] Type message_type)
		{
			var key = key_for(message_type);
			if (!_listeners.ContainsKey(key)) _listeners[key] = new NonNullList<MessageHandler>();
		}

		public void inform_about_message<TMessage>() where TMessage : MailMessage
		{
			inform_about_message(typeof (TMessage));
		}

		[Obsolete("Most callers should use a MailRoom instead of a raw MailIndex. The rest can use the non-generic method. ")]
		public void subscribe<TMessage>([NotNull] Action<TMessage, Action> listener) where TMessage : MailMessage
		{
			subscribe(typeof (TMessage), (m, done) => listener((TMessage) m, done));
		}

		public void subscribe([NotNull] Type message_type, [NotNull] MessageHandler on_message)
		{
			_listeners[key_for(message_type)].Add(on_message);
		}

		public void announce(MailMessage what_happened)
		{
			var when_done_notification = WaitableCounter.non_counting();
			_announce_to_specific_listeners(what_happened, when_done_notification);
		}

		public void announce_and_notify_when_done([NotNull] MailMessage what_happened, [NotNull] Action when_done)
		{
			var when_done_notification = WaitableCounter.starting_at(0, when_done);
			_announce_to_specific_listeners(what_happened, when_done_notification);
		}

		private void _announce_to_specific_listeners([NotNull] MailMessage what_happened,
			[NotNull] WaitableCounter when_done_notification)
		{
			var key = key_for(what_happened.GetType());
			NonNullList<MessageHandler> recipients;
			if (!_listeners.TryGetValue(key, out recipients))
			{
				throw new InvalidOperationException(
					string.Format(
						"Attempted to send a non-registered message. Make sure you register all intended messages with the ConstructionSite before creating the MailRoom. Attempted to send {0} to {1}.",
						what_happened.GetType()
							.Name,
						name));
			}
			_send_to_all(recipients, what_happened, when_done_notification);
		}

		private void _send_to_all([NotNull] NonNullList<MessageHandler> listeners,
			[NotNull] MailMessage what_happened,
			[NotNull] WaitableCounter when_done_notification)
		{
			listeners.ToArray()
				.each(recipient => {
					when_done_notification.begin();
					// ReSharper disable PossibleNullReferenceException
					recipient // ReSharper restore PossibleNullReferenceException
						(what_happened, when_done_notification.done);
				});
		}

		[NotNull]
		private static string key_for([NotNull] Type message_type)
		{
			return message_type.Name;
		}

		public override string ToString()
		{
			return string.Format("Mail room that is {0}.", name);
		}
	}
}
