// For.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using Fools.cs.Api;
using Fools.cs.Api.CommandLineApp;
using Fools.cs.Utilities;

namespace core_compile
{
	public static class For
	{
		public static void my_amusement([NotNull] MissionLocation agent)
		{
			CommandLineProgram<CompilerUserInteractionModel>.show_him_what_you_do(agent);
			CompileProjects.show_him_what_you_do(agent);
			CompileOneProject.show_him_what_you_do(agent);
		}
	}
}
