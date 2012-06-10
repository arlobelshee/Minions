using Microsoft.Cci.MutableCodeModel;

namespace Fools.Declaration.Native
{
	internal class NativeUserDefinedType : UserDefinedType
	{
		private readonly NativeNamespace _ns;
		private readonly NamedTypeDefinition _target;

		public NativeUserDefinedType(NativeNamespace ns, NamedTypeDefinition target)
		{
			_ns = ns;
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }
		public override Namespace name_space { get { return _ns; } }
	}
}