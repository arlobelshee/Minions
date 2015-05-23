// CompilationMode.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Fools.cs.Utilities;

namespace core_compile
{
	public class CompilationMode
	{
		[NotNull] public readonly string build_dir_name;
		[NotNull] private readonly string _debug_settings;
		[NotNull] private readonly string _defines;

		[NotNull] public static readonly CompilationMode DEBUG = new CompilationMode("Debug",
			"-g --debug:full --optimize-",
			new[] {"DEBUG"});

		[NotNull] public static readonly CompilationMode RELEASE = new CompilationMode("Release",
			"--debug:pdbonly --optimize+",
			new string[] {});

		[NotNull] private static readonly DirectoryInfo _assembly_directory =
			new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
				.Location));

		[NotNull] public static readonly DirectoryInfo FSHARP_COMPILER_DIR =
			_assembly_directory.GetDirectories("msbuild_helpers")
				.First();

		[NotNull] private static readonly FileInfo _fsharp_compiler = FSHARP_COMPILER_DIR.GetFiles("fsc.exe")
			.First();

		private CompilationMode([NotNull] string build_dir_name,
			[NotNull] string debug_settings,
			[NotNull] IEnumerable<string> defines)
		{
			this.build_dir_name = build_dir_name;
			_debug_settings = debug_settings;
			_defines = string.Join(" ", defines.Select(d => string.Format("--define:{0}", d)));
		}

		private const string _args_template =
			@"-o:obj\{0}\fsharp_console_app.exe {1} --noframework {2} --define:TRACE --doc:bin\{0}\fsharp_console_app.XML --platform:anycpu32bitpreferred {3} --target:exe --warn:3 --warnaserror:76 --fullpaths --flaterrors --subsystemversion:6.00 --highentropyva+ .AssemblyAttributes.fs {4}";

		[NotNull]
		public FSharpCompilation compile([NotNull] FSharpProject source)
		{
			var compiler_command = new ProcessStartInfo(_fsharp_compiler.FullName,
				_command_line(source.files_to_compile, source.references())) {
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					WorkingDirectory = source.source_root.Path.Absolute
				};
			return new FSharpCompilation(source.source_root, compiler_command, this);
		}

		[NotNull]
		private string _command_line([NotNull] IEnumerable<string> files_to_compile,
			[NotNull] IEnumerable<AssemblyReference> assemblies)
		{
			var to_compile = string.Join(" ", files_to_compile);
			var references = String.Join(" ", assemblies);
			return String.Format(_args_template, build_dir_name, _debug_settings, _defines, references, to_compile);
		}
	}
}
