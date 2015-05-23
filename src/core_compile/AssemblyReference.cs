// AssemblyReference.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using Fools.cs.Utilities;
using Simulated;
using Simulated._Fs;

namespace core_compile
{
	public class AssemblyReference
	{
		[NotNull] private readonly FsFile _lib;

		private AssemblyReference([NotNull] FsDirectory dir, [NotNull] string assembly_name)
		{
			_lib = dir.File(assembly_name);
		}

		private const string _fsharp_assemblies =
			@"C:\Program Files (x86)\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.6\4.3.1.0";

		private const string _net_framework_assemblies =
			@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6";

		[NotNull]
		public static AssemblyReference dot_net([NotNull] string assembly_name, [NotNull] FileSystem fs)
		{
			return new AssemblyReference(fs.Directory(_net_framework_assemblies), assembly_name);
		}

		[NotNull]
		public static AssemblyReference fsharp([NotNull] string assembly_name, [NotNull] FileSystem fs)
		{
			return new AssemblyReference(fs.Directory(_fsharp_assemblies), assembly_name);
		}

		public override string ToString()
		{
			return string.Format("-r:\"{0}\"", _lib.FullPath.Absolute);
		}
	}
}
