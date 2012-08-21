using System.IO;
using System.Threading.Tasks;

namespace Fools.Platform
{
	public class Output : Minion
	{
		private readonly Stream _destination;
		private readonly int _block_size;
		private Rope.BlockSequence _current_output;

		private Output(MinionBoss task_master, Stream destination, int block_size) : base(task_master)
		{
			_destination = destination;
			_block_size = block_size;
		}

		public static Minion writing_to_stream(MinionBoss minion_runtime, Stream destination, int block_size = 1024*32)
		{
			return new Output(minion_runtime, destination, block_size);
		}

		public override WorkSchedule do_thy_masters_bidding(Message message)
		{
			if(!message.is_unnamed_value()) return WorkSchedule.Continue;

			_current_output = message.value.to_user_string().enumerate_bytes(_block_size);
			var continuation = _write_one_block();

			return new WorkSchedule(continuation.Length == 0, continuation);
		}

		private Continuation[] _write_one_block()
		{
			var result = _current_output.next();
			if(!result.HasValue)
			{
				resume_handling_messages();
				return WorkSchedule.NO_WORK_TO_DO;
			}
			var write_operation = task_master.schedule_async_work(this, _destination.BeginWrite, _destination.EndWrite, result.Value.data, 0, result.Value.length);
			write_operation.then(_write_one_block);
			return new[]
			       {
			       	write_operation
			       };
		}

		private void _write_next_block()
		{
			_write_one_block();
		}
	}
}
