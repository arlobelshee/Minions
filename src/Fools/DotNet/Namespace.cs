using Fools.DotNet.Native;

namespace Fools.DotNet
{
	public abstract class Namespace
	{
		public abstract string name { get; }
		public abstract TypeDefinition get_type(string type_name);
	}
}