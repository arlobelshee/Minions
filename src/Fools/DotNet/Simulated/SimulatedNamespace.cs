using System.Collections.Generic;
using Fools.DotNet.Native;

namespace Fools.DotNet.Simulated
{
	internal class SimulatedNamespace : Namespace
	{
		private readonly SimulatedLibrary _assembly;
		private readonly string _name;

		private readonly Dictionary<string, Definition> _members = new Dictionary<string, Definition>();

		public SimulatedNamespace(SimulatedLibrary assembly, string name)
		{
			_assembly = assembly;
			_name = name;
		}

		public override string name { get { return _name; } }

		public override FrameDefinition get_continuation_definition(string type_name)
		{
			return (FrameDefinition) _members[type_name];
		}

		public override FrameDefinition ensure_continuation_definition_exists(string type_name)
		{
			Definition result;
			return _members.TryGetValue(type_name, out result)
				? (FrameDefinition) result
				: _remember(new SimulatedFrameDefinition(this, type_name));
		}

		public override UserDefinedType get_udt(string type_name)
		{
			return (UserDefinedType) _members[type_name];
		}

		public override UserDefinedType ensure_udt_exists(string type_name)
		{
			Definition result;
			return _members.TryGetValue(type_name, out result)
				? (UserDefinedType) result
				: _remember(new SimulatedUserDefinedType(this, type_name));
		}

		private SimulatedFrameDefinition _remember(FrameDefinition simulated_frame_definition)
		{
			return (SimulatedFrameDefinition) (_members[simulated_frame_definition.name] = simulated_frame_definition);
		}

		private SimulatedUserDefinedType _remember(UserDefinedType simulated_frame_definition)
		{
			return (SimulatedUserDefinedType) (_members[simulated_frame_definition.name] = simulated_frame_definition);
		}
	}
}
