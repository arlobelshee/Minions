using System.Threading.Tasks;

namespace Fools.cs.Tests.Interpretation
{
	public class Fool
	{
		private readonly Interpreter _interpreter;
		private readonly Maybe<Task> _quitting = Maybe<Task>.not();

		public Fool(Interpreter interpreter)
		{
			_interpreter = interpreter;
		}

		public Task stop()
		{
			_quitting.assign(_interpreter.create_task());
			return _quitting.value;
		}
	}
}