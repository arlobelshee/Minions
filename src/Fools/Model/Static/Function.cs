namespace Fools.Model.Static
{
	public class Function
	{
		public Function(StackFrame frame)
		{
			this.frame = frame;
		}

		public StackFrame frame { get; private set; }
	}
}