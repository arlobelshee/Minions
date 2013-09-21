// CompileProjects.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Api;
using Fools.cs.Api.CommandLineApp;
using Fools.cs.Utilities;
using core_compile.Messages;

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

		public static void start_compiling_projects([NotNull] CompileProjects lab,
			AppRun<CompilerUserInteractionModel> message)
		{
			Console.WriteLine("I would be parsing the project file here.");
			lab._mission_control.announce(new AppQuit(AppErrorLevel.Ok));
		}

		public static void finished_one_project(CompileProjects lab, FoolsProjectCompileFinished message)
		{
			throw new NotImplementedException();
		}
	}
}
