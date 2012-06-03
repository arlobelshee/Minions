using Microsoft.Cci.MutableCodeModel;

namespace Fools.DotNet.Native
{
	public class NativeNamespace : Namespace
	{
		private readonly UnitNamespace _target;

		public NativeNamespace(UnitNamespace target)
		{
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }
	}
}