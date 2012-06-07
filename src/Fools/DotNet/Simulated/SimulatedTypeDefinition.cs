namespace Fools.DotNet.Simulated
{
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