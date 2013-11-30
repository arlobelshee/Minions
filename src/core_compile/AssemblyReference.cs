// AssemblyReference.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using Fools.cs.Utilities;

namespace core_compile
{
	public class AssemblyReference
	{
		[NotNull] private readonly string _dir;
		[NotNull] private readonly string _assembly_name;

		public AssemblyReference([NotNull] string dir, [NotNull] string assembly_name)
		{
			_dir = dir;
			_assembly_name = assembly_name;
		}

		private const string _fsharp_assemblies =
			@"C:\Program Files (x86)\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.3.1.0";

		private const string _net_framework_assemblies =
			@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5";

		[NotNull]
		public static AssemblyReference dot_net([NotNull] string assembly_name)
		{
			return new AssemblyReference(_net_framework_assemblies, assembly_name);
		}

		[NotNull]
		public static AssemblyReference fsharp([NotNull] string assembly_name)
		{
			return new AssemblyReference(_fsharp_assemblies, assembly_name);
		}

		public override string ToString()
		{
			return string.Format("-r:\"{0}\\{1}\"", _dir, _assembly_name);
		}
	}
}
