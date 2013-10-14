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
		public delegate void MessageHandler(MailMessage message, Action done);

		[NotNull] private readonly NonNullDictionary<string, NonNullList<MessageHandler>> _listeners =
			new NonNullDictionary<string, NonNullList<MessageHandler>>();

		public void inform_about_message([NotNull] Type message_type)
		{
			var key = key_for(message_type);
			if (!_listeners.ContainsKey(key)) _listeners[key] = new NonNullList<MessageHandler>();
		}

		public void inform_about_message<TMessage>() where TMessage : MailMessage
		{
			inform_about_message(typeof (TMessage));
		}

		public void subscribe<TMessage>([NotNull] Action<TMessage, Action> listener) where TMessage : MailMessage
		{
			subscribe(typeof (TMessage), (m, done) => listener((TMessage) m, done));
		}

		public void subscribe([NotNull] Type message_type, [NotNull] MessageHandler on_message)
		{
			var key = key_for(message_type);
			_listeners[key].Add(on_message);
		}

		public void announce([NotNull] MailMessage what_happened)
		{
			var items_being_processed = WaitableCounter.non_counting();
			_announce_to_specific_listeners(what_happened, items_being_processed);
		}

		public void announce_and_notify_when_done([NotNull] MailMessage what_happened, [NotNull] Action when_done)
		{
			var items_being_processed = WaitableCounter.starting_at(0, when_done);
			_announce_to_specific_listeners(what_happened, items_being_processed);
		}

		private void _announce_to_specific_listeners([NotNull] MailMessage what_happened,
			[NotNull] WaitableCounter items_being_processed)
		{
			var mesage_type = key_for(what_happened.GetType());
			NonNullList<MessageHandler> recipients;
			if (!_listeners.TryGetValue(mesage_type, out recipients)) return;
			_send_to_all(recipients, what_happened, items_being_processed);
		}

		private void _send_to_all([NotNull] NonNullList<MessageHandler> listeners,
			[NotNull] MailMessage what_happened,
			[NotNull] WaitableCounter items_being_processed)
		{
			listeners.ToArray()
				.each(recipient => {
					items_being_processed.begin();
					// ReSharper disable PossibleNullReferenceException
					recipient // ReSharper restore PossibleNullReferenceException
						(what_happened, items_being_processed.done);
				});
		}

		[NotNull]
		private static string key_for([NotNull] Type message_type)
		{
			return message_type.Name;
		}
	}
}
