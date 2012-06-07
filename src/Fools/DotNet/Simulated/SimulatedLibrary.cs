using System.Collections.Generic;
using System.Linq;

namespace Fools.DotNet.Simulated
{
	public class SimulatedLibrary : TypeStore
	{
		private readonly SimulatedCompiler _compiler;
		private readonly string _name;
		private readonly string _file_name;
		private readonly List<TypeStore> _references;
		private readonly Namespace _default_namespace;
		private readonly Dictionary<string, Namespace> _namespaces = new Dictionary<string, Namespace>();

		public SimulatedLibrary(
			SimulatedCompiler compiler, string default_namespace, IEnumerable<TypeStore> initial_references)
		{
			_compiler = compiler;
			_name = default_namespace;
			_file_name = default_namespace + ".dll";
			_references = initial_references.ToList();
			_default_namespace = _add_namespace(default_namespace);
		}

		private SimulatedNamespace _add_namespace(string ns_name)
		{
			var result = new SimulatedNamespace(this, ns_name);
			_namespaces.Add(ns_name, result);
			return result;
		}

		public override string file_name { get { return _file_name; } }
		public override string name { get { return _name; } }
		public override IEnumerable<TypeStore> references { get { return _references; } }
		public override Namespace default_namespace { get { return _default_namespace; } }

		public override Namespace ensure_namespace_exists(string ns_name)
		{
			Namespace result;
			return _namespaces.TryGetValue(ns_name, out result) ? result : _add_namespace(ns_name);
		}

		public override TypeDefinition get_type(TypeName full_name)
		{
			return _namespaces[full_name.namespace_name].get_continuation_definition(full_name.type_name);
		}
	}
}
