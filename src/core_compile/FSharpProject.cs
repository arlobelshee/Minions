// FSharpProject.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Fools.cs.Utilities;

namespace core_compile
{
	public class FSharpProject
	{
		[NotNull] private readonly DirectoryInfo _source_root;
		[NotNull] private readonly List<string> _files_to_compile = new List<string>();

		[NotNull] private readonly List<AssemblyReference> _assembly_references = new List<AssemblyReference> {
			AssemblyReference.fsharp("FSharp.Core.dll"),
			AssemblyReference.dot_net("mscorlib.dll"),
			AssemblyReference.dot_net("System.Core.dll"),
			AssemblyReference.dot_net("System.dll"),
			AssemblyReference.dot_net("System.Numerics.dll")
		};

		public FSharpProject([NotNull] DirectoryInfo source_root)
		{
			_source_root = source_root;
		}

		public void add_file(string relative_file_path)
		{
			_files_to_compile.Add(relative_file_path);
		}

		[NotNull]
		public DirectoryInfo source_root { get { return _source_root; } }

		[NotNull]
		public ReadOnlyCollection<AssemblyReference> references()
		{
			return _assembly_references.AsReadOnly();
		}

		[NotNull]
		public IEnumerable<string> files_to_compile { get { return _files_to_compile.Concat(new[] {"Program.fs"}); } }

		[NotNull]
		public static FSharpProject hello_world()
		{
			var source_root = CompilationMode.FSHARP_COMPILER_DIR;
			var hello_world = new FSharpProject(source_root);
			hello_world.add_file("Methods.fs");
			return hello_world;
		}
	}
}
