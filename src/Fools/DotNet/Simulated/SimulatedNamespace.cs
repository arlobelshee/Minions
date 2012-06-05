using System;

namespace Fools.DotNet.Simulated
{
	public class SimulatedNamespace : Namespace
	{
		private readonly SimulatedLibrary _assembly;
		private readonly string _name;

		public SimulatedNamespace(SimulatedLibrary assembly, string name)
		{
			_assembly = assembly;
			_name = name;
		}

		public override string name { get { return _name; } }

		public override TypeDefinition get_type(string type_name)
		{
			throw new NotImplementedException();
		}
	}
}