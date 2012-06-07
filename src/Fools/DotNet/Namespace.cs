namespace Fools.DotNet
{
	public abstract class Namespace
	{
		public abstract string name { get; }
		public abstract TypeDefinition get_type(string type_name);
		public abstract TypeDefinition ensure_type_exists(string type_name);
	}
}
