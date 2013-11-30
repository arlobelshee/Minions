// FSharpCompilation.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Diagnostics;
using System.IO;
using Fools.cs.Api;
using Fools.cs.Utilities;

namespace core_compile
{
	public class FSharpCompilation
	{
		[NotNull] private readonly DirectoryInfo _source_root;
		[NotNull] private readonly CompilationMode _mode;
		[NotNull] private readonly Process _compiler_process;

		public FSharpCompilation([NotNull] DirectoryInfo source_root,
			[NotNull] ProcessStartInfo compiler_command,
			[NotNull] CompilationMode mode)
		{
			_source_root = source_root;
			_mode = mode;
			_compiler_process = new Process {
				EnableRaisingEvents = true,
				StartInfo = compiler_command
			};
		}

		[NotNull]
		public FSharpCompilation prepare_mission([NotNull] MissionLocation where_to_report_success)
		{
			_compiler_process.ErrorDataReceived += write_data_to(Console.Error);
			_compiler_process.OutputDataReceived += write_data_to(Console.Out);
			_compiler_process.Exited +=
				(sender, args) => where_to_report_success.announce(new FSharpCompileFinished(_compiler_process));
			return this;
		}

		public void begin()
		{
			_create_build_dirs();
			_begin_compilation();
		}

		private static DataReceivedEventHandler write_data_to(TextWriter destination)
		{
			return (sender, args) => { if (destination != null && args != null) destination.WriteLineAsync(args.Data); };
		}

		private void _begin_compilation()
		{
			_compiler_process.Start();
			_compiler_process.BeginErrorReadLine();
			_compiler_process.BeginOutputReadLine();
		}

		private void _create_build_dirs()
		{
			_source_root.CreateSubdirectory(Path.Combine("bin", _mode.build_dir_name));
			_source_root.CreateSubdirectory(Path.Combine("obj", _mode.build_dir_name));
		}
	}
}
