// MessageDelivery.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Fools.cs.Api;
using Fools.cs.Interpret;
using Fools.cs.Tests.Support;
using Fools.cs.Utilities;
using NUnit.Framework;

namespace Fools.cs.Tests.CoreLanguage
{
	[TestFixture]
	public class MessageDelivery
	{
		[Test]
		public void announce_and_notify_should_return_immediately_but_only_signal_after_all_recipients_have_completed()
		{
			var test_subject = _create_mail_room();
			test_subject.subscribe<SillyMessage>(async_handler);
			test_subject.announce_and_notify_when_done(new SillyMessage("hi"), _notified.notify);
			_notified.wait_until_count_reaches(1, Consts.TINY_DELAY)
				.Should()
				.BeTrue();
			_log.Should()
				.Equal(new object[] {"Started: hi", "Finished: hi"});
		}

		[Test]
		public void announce_and_notify_should_not_fire_if_tasks_never_finish()
		{
			var test_subject = _create_mail_room();
			test_subject.subscribe<SillyMessage>(non_terminating_handler);
			test_subject.announce_and_notify_when_done(new SillyMessage("hi"), _notified.notify);
			_notified.wait_until_count_reaches(1, Consts.TINY_DELAY)
				.Should()
				.BeFalse();
			_log.Should()
				.Equal(new object[] {"Started: hi"});
		}

		[Test]
		public void announce_should_return_after_all_recipients_have_been_notified_but_before_they_finish()
		{
			var test_subject = _create_mail_room();
			test_subject.subscribe<SillyMessage>(non_terminating_handler);
			test_subject.announce(new SillyMessage("hi"));
			_log.Should()
				.Equal(new object[] {"Started: hi"});
		}

		[Test]
		public void announced_messages_should_be_delivered_to_all_who_have_expressed_interest_in_that_message_type()
		{
			var log = store_message_values();
			var test_subject = _create_mail_room();
			test_subject.subscribe<SillyMessage>(log.accept);
			test_subject.announce(new SillyMessage("1"));
			test_subject.announce(new SillyMessage("2"));
			log.received.Should()
				.Equal(new object[] {"1", "2"});
		}

		[Test]
		public void subscribers_should_only_get_messages_they_asked_for()
		{
			var log = store_message_values();
			var test_subject = _create_mail_room();
			test_subject.subscribe<SillyMessage>(log.accept);
			test_subject.announce(new SillyMessage("silly"));
			test_subject.announce(new SeriousMessage("serious"));
			log.received.Should()
				.Equal(new object[] {"silly"});
		}

		[Test]
		public void
			subscribing_for_a_message_while_processing_that_message_should_result_in_new_subscriber_not_receiving_message()
		{
			var test_subject = _create_mail_room();
			test_subject.subscribe<SillyMessage>(
				new RecursiveSubscriber(3, test_subject, _log).subscribe_recursively_until_counter_expires);
			test_subject.subscribe<SillyMessage>(
				new RecursiveSubscriber(3, test_subject, _log).subscribe_recursively_until_counter_expires);
			test_subject.announce_and_notify_when_done(new SillyMessage("hi"), _notified.notify);
			_notified.wait_until_count_reaches(1, Consts.TINY_DELAY)
				.Should()
				.BeTrue();
			_log.Should()
				.Equal(new object[] {"Counted 3: hi", "Counted 3: hi"});
		}

		[SetUp]
		public void initialize()
		{
			_notified = NonBlockingTally.starting_at(0);
			_log = new List<string>();
		}

		[NotNull]
		private static MailRoom _create_mail_room()
		{
			var test_subject = new MailRoom();
			test_subject.inform_about_message<SillyMessage>();
			test_subject.inform_about_message<SeriousMessage>();
			return test_subject;
		}

		private void async_handler([NotNull] SillyMessage m, [NotNull] Action done)
		{
			_log.Add(string.Format("Started: {0}", m.value));
			new Task(() => {
				_log.Add(string.Format("Finished: {0}", m.value));
				done();
			}).Start();
		}

		private void non_terminating_handler([NotNull] SillyMessage m, [NotNull] Action done)
		{
			_log.Add(string.Format("Started: {0}", m.value));
		}

		[NotNull]
		private static MessageLog<string, SillyMessage> store_message_values()
		{
			// ReSharper disable PossibleNullReferenceException
			return new MessageLog<string, SillyMessage>(m => m.value);
			// ReSharper restore PossibleNullReferenceException
		}

		private class RecursiveSubscriber
		{
			private readonly int _counter;
			[NotNull] private readonly List<string> _log;
			[NotNull] private readonly MailRoom _mail_room_for_recursive_subscriptions;

			public RecursiveSubscriber(int counter,
				[NotNull] MailRoom mail_room_for_recursive_subscriptions,
				[NotNull] List<string> log)
			{
				_counter = counter;
				_mail_room_for_recursive_subscriptions = mail_room_for_recursive_subscriptions;
				_log = log;
			}

			public void subscribe_recursively_until_counter_expires([NotNull] SillyMessage message, [NotNull] Action done)
			{
				_log.Add(string.Format("Counted {0}: {1}", _counter, message.value));
				var next = _counter - 1;
				if (next <= 0)
				{
					done();
					return;
				}
				_recurse(next);
				new Task(() => {
					_recurse(next);
					done();
				}).Start();
			}

			private void _recurse(int next)
			{
				_mail_room_for_recursive_subscriptions.subscribe<SillyMessage>(
					new RecursiveSubscriber(next, _mail_room_for_recursive_subscriptions, _log)
						.subscribe_recursively_until_counter_expires);
			}
		}

		[NotNull] private List<string> _log;
		[NotNull] private NonBlockingTally _notified;
	}

	public class SillyMessage : MailMessage
	{
		public SillyMessage(string value)
		{
			this.value = value;
		}

		public string value { get; private set; }
	}

	public class SeriousMessage : SillyMessage
	{
		public SeriousMessage(string value) : base(value) {}
	}
}
