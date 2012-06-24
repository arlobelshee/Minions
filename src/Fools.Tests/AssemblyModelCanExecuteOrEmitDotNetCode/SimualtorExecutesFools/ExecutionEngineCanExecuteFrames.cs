using NUnit.Framework;

namespace Fools.Tests.AssemblyModelCanExecuteOrEmitDotNetCode.SimualtorExecutesFools
{
	[TestFixture]
	public class ExecutionEngineCanExecuteFrames
	{
		[Test]
		public void frames_execute_and_return_the_next_frame_to_execute()
		{
			var test_subject = new TypeUnsafeFrame();

			// Execution is always done by using Tasks. TPL gives me my continuations stacks.
			// To compile, we take the model and emit functions. We then always call those
			// via task.
			// These functions are limited as follows:
			// * They never call any other functions. All function calls are done by setting up tasks
			//   to execute.
			// * They always return void. Return value binding is incorporated into each function; it
			//   takes a binding argument set as an arg, and passes values into that as it goes along.

			Assert.Fail("I need to figure out how to write this test.");
		}

		[Test, Ignore("Not implemented yet. Have to do other tests first.")]
		public void fool_keeps_track_of_universe_and_loops_executing_frames()
		{
			
		}

		[Test, Ignore("Not implemented yet. Have to do other tests first.")]
		public void scheduler_decides_which_fools_to_execute_on_which_threads()
		{
			
		}

		[Test, Ignore("Not implemented yet. Have to do other tests first.")]
		public void execution_context_chooses_threads_to_run_based_off_computer_resources_and_program_needs()
		{

		}
	}

	public class TypeUnsafeFrame : Continuation
	{
	}

	public abstract class Continuation
	{
	}
}
