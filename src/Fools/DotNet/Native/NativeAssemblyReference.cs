using System.Collections.Generic;
using Microsoft.Cci;

namespace Fools.DotNet.Native
{
	public class NativeAssemblyReference : TypeStore
	{
		private readonly IAssemblyReference _target;

		public NativeAssemblyReference(IAssemblyReference target)
		{
			_target = target;
		}

		public override string file_name { get { return _target.Name.Value; } }
		public override string name { get { return _target.Name.Value; } }

		public override IEnumerable<TypeStore> references { get { return Enumerable<TypeStore>.Empty; } }
		public override Namespace default_namespace { get { return new NativeNamespaceReference(_target.ResolvedModule.NamespaceRoot); } }
	}
}