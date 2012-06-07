namespace Fools.DotNet
{
	public abstract class Namespace
	{
		public abstract string name { get; }
		public abstract TypeDefinition get_continuation_definition(string type_name);
		public abstract TypeDefinition ensure_continuation_definition_exists(string type_name);
	}
}
