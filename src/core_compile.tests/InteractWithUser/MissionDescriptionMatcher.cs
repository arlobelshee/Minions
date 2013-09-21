// MissionDescriptionMatcher.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Linq;
using Fools.cs.Api;
using Fools.cs.Utilities;
using NSubstitute;

namespace core_compile.tests.InteractWithUser
{
	internal class MissionDescriptionMatcher<TLab> : IArgumentMatcher, IDescribeNonMatches where TLab : class
	{
		[NotNull] private readonly NonNullList<Type> _spawning_messages;
		[NotNull] private readonly NonNullList<MissionActivity<TLab>> _activities;

		public MissionDescriptionMatcher([NotNull] NonNullList<Type> spawning_messages,
			[NotNull] NonNullList<MissionActivity<TLab>> activities)
		{
			_spawning_messages = spawning_messages;
			_activities = activities;
		}

		public bool IsSatisfiedBy(object argument)
		{
			var mission = argument as MissionDescription<TLab>;
			if (mission == null) return false;
			return _spawning_messages.sequence_equivalent(mission.spawning_messages)
				&& _activities.sequence_equivalent(mission.activities);
		}

		public override string ToString()
		{
			return string.Format("a mission in a {0} lab", typeof (TLab).Name);
		}

		public string DescribeFor(object argument)
		{
			var mission = argument as MissionDescription<TLab>;
			if (mission == null) return "wrong kind of mission";
			var mismatched_spawn_messages = _spawning_messages.mismatches_with(mission.spawning_messages);
			if (!mismatched_spawn_messages.is_empty) return mismatched_spawn_messages.describe("spawning messages", "Expected but not found", "Found but not expected");
			var mismatched_activities = _activities.mismatches_with(mission.activities);
			if (!mismatched_activities.is_empty) return mismatched_activities.describe("activities", "Expected but not found", "Found but not expected");
			return "some unknown difference in missions";
		}
	}
}
