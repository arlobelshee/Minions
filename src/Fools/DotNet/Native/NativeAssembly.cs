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
		private readonly Dictionary<string, Namespace> _namespaces = new Dictionary<string, Namespace>();

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
			_root_namespace = new RootUnitNamespace
			                  {
			                  	Unit = _assembly
			                  };
			var root = _remember_namespace(_root_namespace);
			_assembly.UnitNamespaceRoot = _root_namespace;
			_default_namespace = _add_namespace(default_namespace);
			root.create_class("<Module>");
		}

		private NativeNamespace _add_namespace(string full_name)
		{
			var ns = new NestedUnitNamespace
			         {
			         	Name = _compiler.name(full_name),
			         	Unit = _assembly,
			         	ContainingUnitNamespace = _root_namespace
			         };
			_root_namespace.Members.Add(ns);
			return _remember_namespace(ns);
		}

		private NativeNamespace _remember_namespace(UnitNamespace ns)
		{
			var result = new NativeNamespace(_compiler, _assembly, ns);
			_namespaces.Add(result.name, result);
			return result;
		}

		public override string file_name { get { return _assembly.ModuleName.Value; } }
		public override string name { get { return _assembly.Name.Value; } }
		public override IEnumerable<TypeStore> references { get { return _assembly.AssemblyReferences.Select(a => new NativeAssemblyReference(_compiler, a)); } }
		public override Namespace default_namespace { get { return _default_namespace; } }

		public TypeDefinition get_type(string full_name)
		{
			return get_type(TypeName.of(full_name));
		}

		public TypeDefinition get_type(TypeName full_name)
		{
			return _namespaces[full_name.namespace_name].get_type(full_name.type_name);
		}
	}
}
