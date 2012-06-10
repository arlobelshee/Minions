using Microsoft.Cci;

namespace Fools.Declaration.Native
{
	internal class NativeFrameDefinitionReference : FrameDefinition
	{
		private readonly NativeNamespaceReference _ns;
		private readonly INamespaceMember _target;

		public NativeFrameDefinitionReference(NativeNamespaceReference ns, INamespaceMember target)
		{
			_ns = ns;
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }
		public override Namespace name_space { get { return _ns; } }
	}
}