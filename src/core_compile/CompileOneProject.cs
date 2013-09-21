// CompileOneProject.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Api;
using Fools.cs.Utilities;
using core_compile.Messages;

namespace core_compile
{
	public class CompileOneProject
	{
		[NotNull] private readonly MissionLocation _mission_control;

		private CompileOneProject([NotNull] MissionLocation mission_control)
		{
			_mission_control = mission_control;
		}

		public static void show_him_what_you_do([NotNull] MissionLocation mission_control)
		{
			var compile_a_project_when_found = NewMission.in_lab(() => new CompileOneProject(mission_control));
			compile_a_project_when_found.send_new_fool_when<FoolsProjectFound>()
				.and_have_it(start_compiling);

			mission_control.send_out_fools_to(compile_a_project_when_found);
		}

		public static void start_compiling(CompileOneProject lab, FoolsProjectFound message)
		{
			throw new NotImplementedException();
		}
	}
}
