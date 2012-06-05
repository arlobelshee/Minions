using Fools.DotNet.Native;
using Fools.DotNet.Simulated;

namespace Fools.DotNet
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
