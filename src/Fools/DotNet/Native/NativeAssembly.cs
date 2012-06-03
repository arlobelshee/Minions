using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.MutableCodeModel;

namespace Fools.DotNet.Native
{
	public class NativeAssembly : TypeStore
	{
		private readonly NativeCompiler _compiler;
		private readonly Assembly _assembly;
		private readonly NativeNamespace _default_namespace;
		private readonly RootUnitNamespace _root_namespace;

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
			_root_namespace = new RootUnitNamespace();
			_assembly.UnitNamespaceRoot = _root_namespace;
			_root_namespace.Unit = _assembly;
			_default_namespace = _add_namespace(default_namespace);
		}

		private NativeNamespace _add_namespace(string full_name)
		{
			var result = new NestedUnitNamespace
										{
											Name = _compiler.name(full_name),
											Unit = _assembly,
											ContainingUnitNamespace = _root_namespace
										};
			_root_namespace.Members.Add(result);
			return new NativeNamespace(result);
		}

		public override string file_name { get { return _assembly.ModuleName.Value; } }
		public override string name { get { return _assembly.Name.Value; } }
		public override IEnumerable<TypeStore> references { get { return _assembly.AssemblyReferences.Select(a => new NativeAssemblyReference(a)); } }
		public override Namespace default_namespace { get { return _default_namespace; } }
	}
}
