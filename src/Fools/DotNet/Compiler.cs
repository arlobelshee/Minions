namespace Fools.DotNet
{
	public abstract class Compiler
	{
		public abstract TypeStore new_library(string default_namespace);
	}
}
