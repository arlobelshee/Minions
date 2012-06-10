using Fools.Declaration.Native;
using Fools.Declaration.Simulated;

namespace Fools.Declaration
{
	public static class DotNetCode
	{
		public static Compiler native()
		{
			return new NativeCompiler();
		}

		public static Compiler simulated()
		{
			return new SimulatedCompiler();
		}
	}
}
