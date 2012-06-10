using System.Collections.Generic;

namespace Fools.Declaration
{
	public abstract class TypeStore
	{
		public abstract string file_name { get; }
		public abstract string name { get; }
		public abstract IEnumerable<TypeStore> references { get; }
		public abstract Namespace default_namespace { get; }
		public abstract Namespace ensure_namespace_exists(string ns_name);
		public abstract FrameDefinition get_type(TypeName full_name);

		public FrameDefinition get_type(string full_name)
		{
			return get_type(TypeName.of(full_name));
		}
	}
}