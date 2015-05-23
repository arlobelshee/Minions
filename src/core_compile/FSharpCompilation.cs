// FSharpCompilation.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Fools.cs.Api;
using Fools.cs.Utilities;
using Simulated;
using Simulated._Fs;

namespace core_compile
{
	public class FSharpCompilation
	{
		[NotNull] private readonly FsDirectory _source_root;
		[NotNull] private readonly CompilationMode _mode;
		[NotNull] private readonly Process _compiler_process;

		public FSharpCompilation([NotNull] FsDirectory source_root,
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

		[NotNull]
		private static DataReceivedEventHandler write_data_to([CanBeNull] TextWriter destination)
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
			var bin = _source_root.Dir("bin")
				.Dir(_mode.build_dir_name);
			var obj = _source_root.Dir("obj")
				.Dir(_mode.build_dir_name);
			Task.WaitAll(bin.EnsureExists(), obj.EnsureExists());
		}

		[NotNull]
		public static FsDirectory tools_and_libs([NotNull] FileSystem file_system)
		{
			return file_system.File(Assembly.GetExecutingAssembly()
				.Location)
				.ContainingFolder.Dir("msbuild_helpers");
		}
	}
}
