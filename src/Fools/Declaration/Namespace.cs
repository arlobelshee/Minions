namespace Fools.Declaration
{
	public abstract class Namespace
	{
		public abstract string name { get; }
		public abstract FrameDefinition get_continuation_definition(string type_name);
		public abstract FrameDefinition ensure_continuation_definition_exists(string type_name);
		public abstract UserDefinedType get_udt(string type_name);
		public abstract UserDefinedType ensure_udt_exists(string type_name);
	}
}