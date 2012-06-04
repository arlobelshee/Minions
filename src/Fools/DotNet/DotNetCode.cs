using Fools.DotNet.Native;

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
			return new NativeCompiler();
		}
	}
}
