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
		private const string _program_file_contents = @"open AllTheThings

[<EntryPoint>]
let main argv =
  say_hello argv
";
		private const string _fsharp_assembly_attributes_contents = @"namespace Microsoft.BuildSettings
  [<System.Runtime.Versioning.TargetFrameworkAttribute("".NETFramework,Version=v4.5"", FrameworkDisplayName="".NET Framework 4.5"")>]
  do ()
";
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
			var tmp = await file_system.TempDirectory;
			var source_root = tmp.Dir("hello_world");
			await source_root.File("Program.fs")
				.Overwrite(_program_file_contents);
			await source_root.File(".AssemblyAttributes.fs").Overwrite(_fsharp_assembly_attributes_contents);

			return new FSharpProject(source_root);
		}
	}
}
