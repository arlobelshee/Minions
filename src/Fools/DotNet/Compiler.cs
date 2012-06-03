namespace Fools.DotNet
{
	public abstract class Compiler
	{
		public abstract ITypeStore new_library(string default_namespace);
	}
}
