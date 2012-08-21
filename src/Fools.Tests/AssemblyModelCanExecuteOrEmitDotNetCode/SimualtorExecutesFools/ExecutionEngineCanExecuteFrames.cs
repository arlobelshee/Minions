﻿using Fools.Declaration.Simulated;
using Fools.Platform;
using NUnit.Framework;

namespace Fools.Tests.AssemblyModelCanExecuteOrEmitDotNetCode.SimualtorExecutesFools
{
	[TestFixture]
	public class ExecutionEngineCanExecuteFrames
	{
		[Test]
		public void execute_frame_that_has_no_subsequent_work_to_do()
		{
			// create a function declaration (no-op body).
			var no_op_fn =
				new SimulatedCompiler().new_library("testy").default_namespace.ensure_continuation_definition_exists("no.op");

			// Create an execution engine (interpreter).
			var interpreter = new FoolInterpreter();

			// Create a bound continuation from the declaration.
			var call = interpreter.load(no_op_fn);

			// Create input and output frames that match this continuation.
			var inputs = new ReadOnlyVoidFrame();
			var outputs = new WriteOnlyRecordingVoidFrame();
			var universes = new Universe[0];

			// Execute the continuation on those inputs and outputs.
			interpreter.interpret(call, universes, inputs, outputs);

			// Nothing should happen.
		}

		[Test]
		public void execute_increment_on_value_and_return_result()
		{
			//// create a function declaration (no-op body).
			//var increment_function =
			//   new SimulatedCompiler().new_library("testy").default_namespace.ensure_continuation_definition_exists("increment");
			//increment_function.body(...);
			//increment_function.inputs(typeof(int));
			//increment_function.outputs(typeof(int));

			//// Create an execution engine (interpreter).
			//var interpreter = new FoolInterpreter();

			//// Create a bound continuation from the declaration.
			//var call = interpreter.load(increment_function);

			//// Create input and output frames that match this continuation.
			//var inputs = new ReadOnlyFrame<int>(6);
			//var outputs = new WriteOnlyRecordingFrame<int>();
			//var universes = new Universe[0];

			//// Execute the continuation on those inputs and outputs.
			//interpreter.interpret(call, universes, inputs, outputs);

			//outputs.positional_value(0).Should().Be(7);
		}

		[Test, Ignore]
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
			// * Incoming args: a binding set of parameters & a set of universes. Compiler already
			//   verified all universe access controls were correct and scheduler is ensuring that
			//   critical sections are long enough. So no need for access control within the function.

			// Alternatively, we manage our own continuations. This is more work for me, but
			// is the only way that I can think of having sufficient scheduler control to run
			// minions only when their universes are free.
			//
			// Unless I can do it with a custom work scheduler for TPL. Will examine.

			// If doing it myself, then each function's signature is
			//   set<Continuations[not started]> func(BindingSet args, BindingSet return_values)
			//
			// Functions still have the limitation that they never call any other functions. They
			// just return any subsequent calls as continuations. These could be configured
			// with arbitrary chaining.

			// Next step: write a platform fool to wrap a stream. And then a function that sends a
			// C# string to that stream. Will extend it (a lot) more later.
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
