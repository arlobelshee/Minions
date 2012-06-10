namespace Fools.Declaration.Simulated
{
	public class SimulatedCompiler : Compiler
	{
		private readonly SimulatedLibrary _mscorlib;

		public SimulatedCompiler()
		{
			_mscorlib = new SimulatedLibrary(
				this,
				"mscorlib",
				new TypeStore[]
				{
				});
		}

		public override TypeStore new_library(string default_namespace)
		{
			return new SimulatedLibrary(
				this,
				default_namespace,
				new[]
				{
					_mscorlib
				});
		}
	}
}