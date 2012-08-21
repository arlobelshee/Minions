using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fools.Platform
{
	public class MinionCommandCenter
	{
		private readonly UniverseAwareScheduler _scheduler;
		private readonly TaskFactory _task_factory;
		private MinionBoss _boss;

		public MinionCommandCenter()
		{
			var cancel = new CancellationTokenSource();
			_scheduler = new UniverseAwareScheduler();
			_task_factory = new TaskFactory(cancel.Token, TaskCreationOptions.None, TaskContinuationOptions.ExecuteSynchronously, _scheduler);
			_boss = new MinionBoss(_task_factory);
		}
	}

	public class MinionBoss
	{
		private readonly TaskFactory _task_factory;

		public MinionBoss(TaskFactory task_factory)
		{
			_task_factory = task_factory;
		}

		public void i_await_your_bidding(Minion minion)
		{
		}

		public Continuation schedule_async_work<TArg0, TArg1, TArg2>(Minion worker,
			Func<TArg0, TArg1, TArg2, AsyncCallback, object, IAsyncResult> begin_method,
			Action<IAsyncResult> end_method,
			TArg0 arg0,
			TArg1 arg1,
			TArg2 arg2)
		{
			return new Continuation(this, worker.universe_awareness, _task_factory.FromAsync(begin_method, end_method, arg0, arg1, arg2, null));
		}
	}

	public class Continuation
	{
		public MinionBoss executer { get; set; }
		public UniverseAccessSet universe_awareness { get; private set; }
		private readonly Task _implementation;

		public Continuation(MinionBoss executer, UniverseAccessSet universe_awareness, Task implementation)
		{
			this.executer = executer;
			this.universe_awareness = universe_awareness;
			_implementation = implementation;
		}

		public Continuation then(Func<Continuation[]> next_task)
		{
			return this;
		}
	}

	public struct WorkSchedule
	{
		public readonly bool continue_processing_events;
		public readonly Continuation[] continuations;

		public static WorkSchedule Continue = new WorkSchedule(true);

		public static readonly Continuation[] NO_WORK_TO_DO = new Continuation[0];

		public WorkSchedule(bool continue_processing_events)
		{
			this.continue_processing_events = continue_processing_events;
			continuations = NO_WORK_TO_DO;
		}

		public WorkSchedule(bool continue_processing_events, Maybe<Continuation> continuation)
		{
			this.continue_processing_events = continue_processing_events;
			if(continuation.HasValue)
				continuations = new[]
				                {
				                	continuation.Value
				                };
			else
				continuations = NO_WORK_TO_DO;
		}

		public WorkSchedule(bool continue_processing_events, Continuation[] continuations)
		{
			this.continue_processing_events = continue_processing_events;
			this.continuations = continuations;
		}
	}
}
