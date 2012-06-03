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
			_assembly = _create_assembly_impl(default_namespace, kind);
			add_reference(compiler.mscorlib);
			_root_namespace = _create_root_namespace();
			_default_namespace = _add_namespace(default_namespace);
			var root = _remember_namespace(_root_namespace);
			root.create_class("<Module>");
		}

		public override string file_name { get { return _assembly.ModuleName.Value; } }
		public override string name { get { return _assembly.Name.Value; } }
		public override IEnumerable<TypeStore> references { get { return _assembly.AssemblyReferences.Select(a => new NativeAssemblyReference(_compiler, a)); } }
		public override Namespace default_namespace { get { return _default_namespace; } }

		public void add_reference(IAssemblyReference assembly_name)
		{
			_assembly.AssemblyReferences.Add(assembly_name);
		}

		public TypeDefinition get_type(string full_name)
		{
			return get_type(TypeName.of(full_name));
		}

		public TypeDefinition get_type(TypeName full_name)
		{
			return _namespaces[full_name.namespace_name].get_type(full_name.type_name);
		}

		private Assembly _create_assembly_impl(string default_namespace_name, ModuleKind kind)
		{
			return new Assembly
			       {
			       	Name = _compiler.name(default_namespace_name),
			       	ModuleName = _compiler.name(default_namespace_name + ".dll"),
			       	Kind = kind,
			       	TargetRuntimeVersion = "4.0"
			       };
		}

		private RootUnitNamespace _create_root_namespace()
		{
			var result = new RootUnitNamespace
			             {
			             	Unit = _assembly
			             };
			_assembly.UnitNamespaceRoot = result;
			return result;
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
	}
}
