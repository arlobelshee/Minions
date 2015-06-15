// CompileProjects.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Diagnostics;
using core_compile.Messages;
using Fools.cs.Api;
using Fools.cs.Api.CommandLineApp;
using Fools.cs.Utilities;
using Simulated;

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
				.whenever<FoolsProjectCompileFinished>(finished_one_project);
			mission_control.send_out_fools_to(watch_for_projects_to_compile);
		}

		public static async void start_compiling_projects([NotNull] CompileProjects lab,
			[NotNull] AppRun<CompilerUserInteractionModel> message)
		{
			Console.WriteLine("I would be handling the command line arguments here to find all projects.");
			Console.WriteLine("#HACK - Instead I'm going to pretend it had just one project.");

			var file_system = FileSystem.Real();
			var tmp = await file_system.TempDirectory;
			Debug.Assert(tmp != null, "tmp != null");
			var project_directory = tmp.Dir("hello_world");
			lab._mission_control.announce(new FoolsProjectFound(project_directory));
		}

		public static void finished_one_project([NotNull] CompileProjects lab, [NotNull] FoolsProjectCompileFinished message)
		{
			Console.WriteLine("#HACK - I'm pretending that there's only one project, so done now. Press ENTER to quit.");
			Console.ReadLine();
			lab._mission_control.announce(new AppQuit(AppErrorLevel.Ok));
		}
	}

	public class FSharpCompileFinished : MailMessage
	{
		[NotNull]
		public Process compile { get; set; }

		public FSharpCompileFinished([NotNull] Process compile)
		{
			this.compile = compile;
		}
	}
}
