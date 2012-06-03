using System.Collections.Generic;
using Microsoft.Cci;

namespace Fools.DotNet.Native
{
	public class NativeAssemblyReference : TypeStore
	{
		private readonly IAssemblyReference _target;
		private readonly NativeCompiler _compiler;

		public NativeAssemblyReference(NativeCompiler compiler, IAssemblyReference target)
		{
			_target = target;
			_compiler = compiler;
		}

		public override string file_name { get { return _target.Name.Value; } }
		public override string name { get { return _target.Name.Value; } }

		public override IEnumerable<TypeStore> references { get { return Enumerable<TypeStore>.Empty; } }
		public override Namespace default_namespace { get { return new NativeNamespaceReference(_compiler, _target.ResolvedModule.NamespaceRoot); } }
	}
}