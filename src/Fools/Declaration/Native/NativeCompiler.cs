using Microsoft.Cci;

namespace Fools.Declaration.Native
{
	public class NativeCompiler : Compiler
	{
		private readonly PeReader.DefaultHost _host;
		private readonly INameTable _names;

		public NativeCompiler()
		{
			_host = new PeReader.DefaultHost();
			_names = _host.NameTable;
			mscorlib = _host.LoadAssembly(_host.CoreAssemblySymbolicIdentity);
		}

		public IAssemblyReference mscorlib { get; private set; }
		public IInternFactory intern_factory { get { return _host.InternFactory; } }

		public override TypeStore new_library(string default_namespace)
		{
			return new NativeAssembly(this, default_namespace, ModuleKind.DynamicallyLinkedLibrary);
		}

		public IName name(string name_string)
		{
			return _names.GetNameFor(name_string);
		}
	}
}
