// ExecuteContinuations.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Concurrent;
using System.Threading;
using FluentAssertions;
using Fools.cs.Api;
using Fools.cs.Utilities;
using NUnit.Framework;

namespace Fools.cs.Tests.PlatformLowLevel
{
	[TestFixture]
	public class ExecuteContinuations
	{
		[Test]
		public void fools_should_execute_in_the_background()
		{
			using (var test_subject = FoolFactory.using_background_threads())
			{
				using (var done = new ManualResetEventSlim(false))
				{
					var first = test_subject.create_fool("one");
					var second = test_subject.create_fool("two");
					var third = test_subject.create_fool("three");

					var finished = NonBlockingBarrier.starting_at(3, done.Set);
					var halfway_done = NonBlockingBarrier.starting_at(3,
						() => {
							first.do_work(got_done, finished.notify);
							second.do_work(got_done, finished.notify);
							third.do_work(got_done, finished.notify);
						});

					first.do_work(_tasks_completed.Enqueue, halfway_done.notify);
					second.do_work(_tasks_completed.Enqueue, halfway_done.notify);

					halfway_done.wait_until_count_reaches(1, _brief_delay)
						.Should()
						.BeTrue();
					halfway_done.remaining_count.Should()
						.Be(1);
					finished.remaining_count.Should()
						.Be(3);

					third.do_work(_tasks_completed.Enqueue, halfway_done.notify);

					done.Wait(_brief_delay)
						.Should()
						.BeTrue();

					_tasks_completed.Should()
						.ContainInOrder(new object[] {"one", "three", "finished one"})
						.And.ContainInOrder(new object[] {"two", "three", "finished two"})
						.And.ContainInOrder(new object[] {"three", "finished three"});
				}
			}
		}

		private void got_done(string val)
		{
			_tasks_completed.Enqueue("finished " + val);
		}

		[Test]
		public void each_fool_should_only_do_one_thing_at_a_time()
		{
			using (var test_subject = FoolFactory.using_background_threads())
			{
				var steps_processed = new ConcurrentQueue<int>();
				var fool = test_subject.create_fool("arbitrary value");
				schedule_work(fool,
					() => {
						wait_until_ready_to_work();
						steps_processed.Enqueue(1);
					});
				schedule_work(fool, () => steps_processed.Enqueue(2));
				schedule_work(fool,
					() => {
						worker_is_done();
						wait_until_ready_to_work();
						steps_processed.Enqueue(3);
						worker_is_done();
					});

				wait_for_worker_to_take_a_step();
				steps_processed.Should()
					.ContainInOrder(1, 2)
					.And.NotContain(3);

				wait_for_worker_to_take_a_step();
				steps_processed.Should()
					.ContainInOrder(1, 2, 3);
			}
		}

		[SetUp]
		public void set_up()
		{
			_tasks_completed = new ConcurrentQueue<string>();
			_worker_done = new ManualResetEventSlim(false);
			_ready_for_worker = new ManualResetEventSlim(false);
			_last_fool_wait_succeeded = false;
		}

		[TearDown]
		public void tear_down()
		{
			_worker_done.Dispose();
			_ready_for_worker.Dispose();
		}

		private void schedule_work([NotNull] Fool<string> fool, [NotNull] Action work)
		{
			fool.do_work(lab => work(), () => { });
		}

		private void wait_for_worker_to_take_a_step()
		{
			_ready_for_worker.Set();
			_worker_done.Wait(_brief_delay)
				.Should()
				.BeTrue();
			_worker_done.Reset();
			_last_fool_wait_succeeded.Should()
				.BeTrue();
		}

		private void worker_is_done()
		{
			_worker_done.Set();
		}

		private void wait_until_ready_to_work()
		{
			_last_fool_wait_succeeded = _ready_for_worker.Wait(_brief_delay);
			_ready_for_worker.Reset();
		}

		private static readonly TimeSpan _brief_delay = TimeSpan.FromMilliseconds(100);
		[NotNull] private ConcurrentQueue<string> _tasks_completed;
		[NotNull] private ManualResetEventSlim _worker_done;
		[NotNull] private ManualResetEventSlim _ready_for_worker;
		private bool _last_fool_wait_succeeded;
	}
}
