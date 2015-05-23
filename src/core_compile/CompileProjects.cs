// CompileProjects.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			CompilationMode.DEBUG.compile(FSharpProject.hello_world().Result)
				.prepare_mission(lab._mission_control)
				.begin();
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
