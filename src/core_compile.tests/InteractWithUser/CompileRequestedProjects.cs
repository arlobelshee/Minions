// CompileRequestedProjects.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using Fools.cs.Api;
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
		}
	}
}
