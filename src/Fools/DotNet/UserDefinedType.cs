namespace Fools.DotNet
{
	public abstract class UserDefinedType : Definition
	{
		public abstract string name { get; }
		public abstract Namespace name_space { get; }

		public override string ToString()
		{
			return string.Format("<Type {0}.{1}>", name_space.name, name);
		}
	}
}