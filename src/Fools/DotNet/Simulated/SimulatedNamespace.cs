using System.Collections.Generic;

namespace Fools.DotNet.Simulated
{
	public class SimulatedNamespace : Namespace
	{
		private readonly SimulatedLibrary _assembly;
		private readonly string _name;

		private readonly Dictionary<string, SimulatedTypeDefinition> _members =
			new Dictionary<string, SimulatedTypeDefinition>();

		public SimulatedNamespace(SimulatedLibrary assembly, string name)
		{
			_assembly = assembly;
			_name = name;
		}

		public override string name { get { return _name; } }

		public override TypeDefinition get_continuation_definition(string type_name)
		{
			return _members[type_name];
		}

		public override TypeDefinition ensure_continuation_definition_exists(string type_name)
		{
			SimulatedTypeDefinition result;
			return _members.TryGetValue(type_name, out result) ? result : _remember(new SimulatedTypeDefinition(this, type_name));
		}

		private SimulatedTypeDefinition _remember(SimulatedTypeDefinition simulated_type_definition)
		{
			return _members[simulated_type_definition.name] = simulated_type_definition;
		}
	}

	public class SimulatedTypeDefinition : TypeDefinition
	{
		private readonly SimulatedNamespace _ns;
		private readonly string _name;

		public SimulatedTypeDefinition(SimulatedNamespace ns, string name)
		{
			_ns = ns;
			_name = name;
		}

		public override string name { get { return _name; } }
		public override Namespace name_space { get { return _ns; } }
	}
}
