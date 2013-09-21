// Missions.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using Fools.cs.Api;
using Fools.cs.Api.CommandLineApp;
using Fools.cs.Utilities;

namespace core_compile
{
	public static class Missions
	{
		public static void we_can_do([NotNull] MissionLocation agent)
		{
			CommandLineProgram<CompilerUserInteractionModel>.show_him_what_you_do(agent);
			CompileProjects.show_him_what_you_do(agent);
			CompileOneProject.show_him_what_you_do(agent);
		}
	}
}
