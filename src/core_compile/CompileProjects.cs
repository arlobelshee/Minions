// CompileProjects.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using core_compile.Messages;
using Fools.cs.Api;
using Fools.cs.Api.CommandLineApp;
using Fools.cs.Utilities;
using Microsoft.Build.Execution;

namespace core_compile
{
	public class CompileProjects
	{
		[NotNull] private readonly MissionLocation _mission_control;

		private CompileProjects([NotNull] MissionLocation mission_control)
		{
			_mission_control = mission_control;
		}

		public static void show_him_what_you_do([NotNull] MissionLocation mission_control)
		{
			var watch_for_projects_to_compile = NewMission.in_lab(() => new CompileProjects(mission_control));
			watch_for_projects_to_compile.send_new_fool_when<AppRun<CompilerUserInteractionModel>>()
				.and_have_it(start_compiling_projects)
				.after_that()
				.whenever<FoolsProjectCompileFinished>(finished_one_project)
				.whenever<FSharpCompileFinished>(report_fsharp_build_results);
			mission_control.send_out_fools_to(watch_for_projects_to_compile);
		}

		public static void start_compiling_projects([NotNull] CompileProjects lab,
			AppRun<CompilerUserInteractionModel> message)
		{
			Console.WriteLine("I would be parsing the project file here.");
			Console.WriteLine("Instead I'm going to compile some F#.");

			// Note: fsc requires the directories to aleady exist. Make them before calling.

			const string fsharp_assemblies =
				@"C:\Program Files (x86)\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.3.1.0";
			const string net_framework_assemblies =
				@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5";
			var debug_directory = "Debug";
			var debug_debug_settings = @"-g --debug:full --optimize-";
			var debug_defines = @"--define:DEBUG";
			var files_to_compile = @"Methods.fs Program.fs";
			var references = (@"-r:""" + fsharp_assemblies + @"\FSharp.Core.dll"" -r:""" + net_framework_assemblies
									+ @"\mscorlib.dll"" -r:""" + net_framework_assemblies + @"\System.Core.dll"" -r:""" + net_framework_assemblies
									+ @"\System.dll"" -r:""" + net_framework_assemblies + @"\System.Numerics.dll""");
			var debug_fsc_command_line =
				string.Format(
					@"-o:obj\{0}\fsharp_console_app.exe {1} --noframework {2} --define:TRACE --doc:bin\{0}\fsharp_console_app.XML --platform:anycpu32bitpreferred {3} --target:exe --warn:3 --warnaserror:76 --fullpaths --flaterrors --subsystemversion:6.00 --highentropyva+ .AssemblyAttributes.fs {4}",
					debug_directory,
					debug_debug_settings,
					debug_defines,
					references,
					files_to_compile);
			var release_fsc_command_line =
				@"-o:obj\Release\fsharp_console_app.exe --debug:pdbonly --noframework --define:TRACE --doc:bin\Release\fsharp_console_app.XML --optimize+ --platform:anycpu32bitpreferred -r:""C:\Program Files (x86)\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.3.1.0\FSharp.Core.dll"" -r:""C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\mscorlib.dll"" -r:""C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Core.dll"" -r:""C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll"" -r:""C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Numerics.dll"" --target:exe --warn:3 --warnaserror:76 --fullpaths --flaterrors --subsystemversion:6.00 --highentropyva+ .AssemblyAttributes.fs Methods.fs Program.fs";

			var fsharp_build_dir = new DirectoryInfo(Directory.GetCurrentDirectory()).GetDirectories("msbuild_helpers").First();
			Debug.Assert(fsharp_build_dir != null, "fsharp_build_dir != null");
			fsharp_build_dir.CreateSubdirectory("bin/" + debug_directory);
			fsharp_build_dir.CreateSubdirectory("obj/" + debug_directory);
			var compiler_command = new ProcessStartInfo(fsharp_build_dir.GetFiles("fsc.exe").First().FullName, debug_fsc_command_line) {
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				WorkingDirectory = fsharp_build_dir.FullName
			};
			var compile = new Process {EnableRaisingEvents = true, StartInfo = compiler_command};
			compile.ErrorDataReceived += write_data_to(Console.Error);
			compile.OutputDataReceived += write_data_to(Console.Out);
			compile.Exited += (sender, args) => lab._mission_control.announce(new FSharpCompileFinished(compile));
			compile.Start();
			compile.BeginErrorReadLine();
			compile.BeginOutputReadLine();
		}

		private static DataReceivedEventHandler write_data_to(TextWriter destination)
		{
			return (sender, args) => { if (destination != null && args != null) destination.WriteLineAsync(args.Data); };
		}

		public static void report_fsharp_build_results([NotNull] CompileProjects lab, [NotNull] FSharpCompileFinished message)
		{
			Console.WriteLine("Build completed.");
			message.compile.Dispose();
			Console.ReadLine();
			lab._mission_control.announce(new AppQuit(AppErrorLevel.Ok));
		}

		public static void finished_one_project(CompileProjects lab, FoolsProjectCompileFinished message)
		{
			throw new NotImplementedException();
		}

		private static BuildRequestData build_project_file([NotNull] string project_file_path)
		{
			var global_properties = new Dictionary<string, string>();
			global_properties["Configuration"] = "Release";
			global_properties["Platform"] = "AnyCpu";
			var command_line_app = new ProjectInstance(project_file_path, global_properties, null);
			var what_to_build = new BuildRequestData(command_line_app, new[] {"Build"});
			return what_to_build;
		}
	}

	public class FSharpCompileFinished : MailMessage
	{
		[NotNull]
		public Process compile { get; set; }

		[NotNull]
		public BuildSubmission submission { get; set; }

		[NotNull]
		public BuildManager build { get; set; }

		public FSharpCompileFinished([NotNull] BuildSubmission submission, [NotNull] BuildManager build)
		{
			this.submission = submission;
			this.build = build;
		}

		public FSharpCompileFinished(Process compile)
		{
			this.compile = compile;
		}
	}
}
