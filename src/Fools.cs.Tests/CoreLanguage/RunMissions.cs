// RunMissions.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Concurrent;
using System.Linq;
using FluentAssertions;
using Fools.cs.Api;
using Fools.cs.Tests.Support;
using Fools.cs.Utilities;
using NUnit.Framework;

namespace Fools.cs.Tests.CoreLanguage
{
	[TestFixture]
	public class RunMissions
	{
		[Test]
		public void mission_control_should_execute_mission_parts_when_messages_arrive()
		{
			using (var fools = new FoolSupplyHouse())
			{
				var done = NonBlockingTally.starting_at(0);
				var test_subject = fools.build_me_a_city(_offer_to_raid_the_elves);
				test_subject.city_hall.announce(new ElvesFound());
				test_subject.city_hall.announce_and_notify_when_done(new SayGo(), done.notify);
				done.wait_until_count_reaches(1, Consts.BRIEF_DELAY)
					.Should()
					.BeTrue();
				should_have_spawned_orcs(1);
				all_orcs_should_have_raided();
			}
		}

		[Test]
		public void mission_control_should_spawn_missions_parts_when_spawn_messages_arrive()
		{
			using (var fools = new FoolSupplyHouse())
			{
				var done = NonBlockingTally.starting_at(0);
				var test_subject = fools.build_me_a_city(_offer_to_raid_the_elves);
				should_be_no_orcs();
				test_subject.city_hall.announce(new ElvesFound());
				test_subject.city_hall.announce_and_notify_when_done(new ElvesFound(), done.notify);
				done.wait_until_count_reaches(1, Consts.BRIEF_DELAY)
					.Should()
					.BeTrue();
				should_have_spawned_orcs(2);
			}
		}

		[Test, Ignore("Not implemented")]
		public void two_diferent_missions_can_spawn_from_same_message()
		{
			throw new NotImplementedException();
		}

		[SetUp]
		public void init()
		{
			_orcs = new ConcurrentQueue<OrcishRaidProgress>();
			_number_of_orcs_raiding = NonBlockingTally.starting_at(0);
		}

		private void _offer_to_raid_the_elves([NotNull] MissionLocation mission_location)
		{
			mission_location.send_out_fools_to(orc_raid());
		}

		[NotNull]
		private MissionDescription<OrcishRaidProgress> orc_raid()
		{
			var raid = NewMission.in_lab(() => new OrcishRaidProgress(9));
			raid.send_new_fool_when<ElvesFound>()
				.and_have_it(_start_new_raid)
				.after_that()
				.whenever<SayGo>(_begin_raiding);
			return raid;
		}

		private void should_have_spawned_orcs(int count)
		{
			_number_of_orcs_raiding.wait_until_count_reaches(count, Consts.BRIEF_DELAY)
				.Should()
				.BeTrue();
		}

		private void all_orcs_should_have_raided()
		{
			_orcs.ToList()
				.Should()
				.OnlyContain(o => o.went_raiding);
		}

		private static void _begin_raiding([NotNull] OrcishRaidProgress lab, [NotNull] SayGo message)
		{
			lab.went_raiding = true;
		}

		private void _start_new_raid([NotNull] OrcishRaidProgress lab, [NotNull] ElvesFound message)
		{
			_orcs.Enqueue(lab);
			_number_of_orcs_raiding.notify();
		}

		private void should_be_no_orcs()
		{
			_orcs.Count.Should()
				.Be(0);
		}

		private class OrcishRaidProgress
		{
			internal bool went_raiding;

			// ReSharper disable UnusedParameter.Local
			public OrcishRaidProgress(int value_just_to_make_sure_lab_is_not_default_constructed_by_framework) {}
			// ReSharper restore UnusedParameter.Local

			public override string ToString()
			{
				return string.Format("went_raiding: {0}", went_raiding);
			}
		}

		[NotNull] private ConcurrentQueue<OrcishRaidProgress> _orcs;
		[NotNull] private NonBlockingTally _number_of_orcs_raiding;
	}

	internal class SayGo : MailMessage {}

	internal class ElvesFound : MailMessage {}
}
