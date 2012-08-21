namespace Fools.Platform
{
	public class Message
	{
		public bool is_unnamed_value()
		{
			return true;
		}

		public FoolValue value { get; private set; }
	}

	public abstract class FoolValue
	{
		public abstract Rope to_user_string();
	}
}