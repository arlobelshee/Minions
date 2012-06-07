using System.Collections.Generic;

namespace Fools.DotNet.Simulated
{
	public class SimulatedNamespace : Namespace
	{
		private readonly SimulatedLibrary _assembly;
		private readonly string _name;

		private readonly Dictionary<string, SimulatedFrameDefinition> _members =
			new Dictionary<string, SimulatedFrameDefinition>();

		public SimulatedNamespace(SimulatedLibrary assembly, string name)
		{
			_assembly = assembly;
			_name = name;
		}

		public override string name { get { return _name; } }

		public override FrameDefinition get_continuation_definition(string type_name)
		{
			return _members[type_name];
		}

		public override FrameDefinition ensure_continuation_definition_exists(string type_name)
		{
			SimulatedFrameDefinition result;
			return _members.TryGetValue(type_name, out result) ? result : _remember(new SimulatedFrameDefinition(this, type_name));
		}

		private SimulatedFrameDefinition _remember(SimulatedFrameDefinition simulated_frame_definition)
		{
			return _members[simulated_frame_definition.name] = simulated_frame_definition;
		}
	}
}
