using Microsoft.Cci;

namespace Fools.DotNet.Native
{
	public class NativeFrameDefinitionReference : FrameDefinition
	{
		private readonly NativeNamespaceReference _namespace;
		private readonly INamespaceMember _target;

		public NativeFrameDefinitionReference(NativeNamespaceReference @namespace, INamespaceMember target)
		{
			_namespace = @namespace;
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }
		public override Namespace name_space { get { return _namespace; } }
	}
}