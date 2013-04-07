namespace Fools.cs.Tests.Interpretation
{
	public class Universe
	{
		public enum Access
		{
			ReadWrite,
			ReadOnly,
			WriteOnly
		}

		internal Universe(Interpreter interpreter)
		{
			interpreter.remember(this);
		}

		internal void destroy()
		{
		}
	}
}
