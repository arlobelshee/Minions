using Microsoft.Cci;

namespace Fools.DotNet.Native
{
	public class NativeNamespaceReference : Namespace
	{
		private readonly INamespaceDefinition _target;

		public NativeNamespaceReference(INamespaceDefinition target)
		{
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }
	}
}