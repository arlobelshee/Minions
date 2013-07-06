// LogToStreamMission.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using FluentAssertions;
using Fools.cs.builtins;
using Fools.cs.builtins.ProgramShells;
using Fools.cs.messages;
using NUnit.Framework;

namespace Fools.cs.Tests.Platform
{
	[TestFixture]
	public class LogToStreamMission
	{
		[Test]
		public void fools_on_this_mission_should_listen_for_logging_messages()
		{
			var subscriptions = MissionLogToStream.interesting_messages();
			subscriptions.Should()
				.Contain(MessageSubscription.to<Write__Normal>(MissionLogToStream.send_message));
		}
	}
}
