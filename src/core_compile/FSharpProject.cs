// FSharpProject.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Fools.cs.Utilities;
using Simulated;
using Simulated._Fs;

namespace core_compile
{
	public class FSharpProject
	{
		[NotNull] private readonly FsDirectory _source_root;
		[NotNull] private readonly List<string> _files_to_compile = new List<string>();
		[NotNull] private readonly List<AssemblyReference> _assembly_references;

		public FSharpProject([NotNull] FsDirectory source_root)
		{
			_source_root = source_root;
			_assembly_references = new List<AssemblyReference> {
				AssemblyReference.fsharp("FSharp.Core.dll", source_root.AllFiles),
				AssemblyReference.dot_net("mscorlib.dll", source_root.AllFiles),
				AssemblyReference.dot_net("System.Core.dll", source_root.AllFiles),
				AssemblyReference.dot_net("System.dll", source_root.AllFiles),
				AssemblyReference.dot_net("System.Numerics.dll", source_root.AllFiles)
			};
		}

		public void add_file(string relative_file_path)
		{
			_files_to_compile.Add(relative_file_path);
		}

		[NotNull]
		public FsDirectory source_root { get { return _source_root; } }

		[NotNull]
		public ReadOnlyCollection<AssemblyReference> references()
		{
			return _assembly_references.AsReadOnly();
		}

		[NotNull]
		public IEnumerable<string> files_to_compile { get { return _files_to_compile.Concat(new[] {"Program.fs"}); } }

		[NotNull]
		public static async Task<FSharpProject> hello_world([NotNull] FileSystem file_system)
		{
			var my_docs = file_system.Directory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
			var source_root = my_docs.Dir("hello_world");
			await source_root.EnsureExists();

			var hello_world = new FSharpProject(source_root);
			hello_world.add_file("Methods.fs");
			return hello_world;
		}
	}
}
