using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fools.Platform
{
	/// <summary>
	/// Encapsulates a set of threads and an ability to ask for work to be done by those threads.
	/// </summary>
	public class UniverseAwareScheduler : TaskScheduler
	{
		/// <summary>
		/// Schedule a task for eventual execution.
		/// </summary>
		/// <param name="task"></param>
		protected override void QueueTask(Task task)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Request that a task be executed right now, on the current thread. The scheduler, of course,
		/// is allowed to do otherwise if it thinks that is better.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskWasPreviouslyQueued"></param>
		/// <returns></returns>
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Used by the debugger to display what is currently scheduled.
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable<Task> GetScheduledTasks()
		{
			return Enumerable.Empty<Task>();
		}
	}
}