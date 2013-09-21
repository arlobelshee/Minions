// CompileRequestedProjects.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using Fools.cs.Api;
using Fools.cs.Api.CommandLineApp;
using Fools.cs.Utilities;
using NSubstitute;
using NUnit.Framework;

namespace core_compile.tests.InteractWithUser
{
	[TestFixture]
	public class CompileRequestedProjects
	{
		[Test]
		public void should_define_all_the_missions()
		{
			var control = Substitute.For<MissionLocation>();
			CompileProjects.submit_missions_to(control);

			control.Received()
				.send_out_fools_to(mission<CompileProjects>()
					.spawn_per<AppRun<CompilerUserInteractionModel>>()
					.when<AppRun<CompilerUserInteractionModel>>(CompileProjects.start_compiling_projects)
					.build());
			control.Received()
				.send_out_fools_to(mission<CompileOneProject>()
					.build());
		}

		[NotNull]
		private MissionVerificationBuilder<TLab> mission<TLab>() where TLab : class
		{
			return new MissionVerificationBuilder<TLab>();
		}
	}
}
