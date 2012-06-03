using System.Collections.Generic;

namespace Fools.DotNet
{
	public abstract class TypeStore
	{
		public abstract string file_name { get; }
		public abstract string name { get; }
		public abstract IEnumerable<TypeStore> references { get; }
		public abstract Namespace default_namespace { get; }
	}
}