using Microsoft.Cci.MutableCodeModel;

namespace Fools.DotNet.Native
{
	internal class NativeTypeDefinition : TypeDefinition
	{
		private readonly NativeNamespace _ns;
		private readonly NamedTypeDefinition _target;

		public NativeTypeDefinition(NativeNamespace ns, NamedTypeDefinition target)
		{
			_ns = ns;
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }
		public override Namespace name_space { get { return _ns; } }
	}
}