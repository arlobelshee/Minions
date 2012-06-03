using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.MutableCodeModel;

namespace Fools.DotNet.Native
{
	public class NativeAssembly : ITypeStore
	{
		private readonly NativeCompiler _compiler;
		private readonly Assembly _assembly;

		public NativeAssembly(NativeCompiler compiler, string default_namespace, ModuleKind kind)
		{
			_compiler = compiler;
			_assembly = new Assembly
			            {
			            	Name = _compiler.name(default_namespace),
			            	ModuleName = _compiler.name(default_namespace + ".dll"),
			            	Kind = kind,
			            	TargetRuntimeVersion = "4.0"
			            };
			_assembly.AssemblyReferences.Add(compiler.mscorlib);
		}

		public string file_name { get { return _assembly.ModuleName.Value; } }
		public string name { get { return _assembly.Name.Value; } }
		public IEnumerable<ITypeStore> references { get { return _assembly.AssemblyReferences.Select(a => new NativeAssemblyReference(a)); } }
	}
}
