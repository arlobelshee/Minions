namespace Fools.DotNet
{
	public abstract class FrameDefinition : Definition
	{
		public abstract string name { get; }
		public abstract Namespace name_space { get; }
	}
}