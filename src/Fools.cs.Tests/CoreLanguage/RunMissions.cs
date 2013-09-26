﻿// RunMissions.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Threading;
using FluentAssertions;
using Fools.cs.Api;
using Fools.cs.Utilities;
using NUnit.Framework;

namespace Fools.cs.Tests.CoreLanguage
{
	[TestFixture]
	public class RunMissions : IDisposable
	{
		[Test]
		public void mission_control_should_execute_mission_parts_when_messages_arrive()
		{
			using (var fools = new FoolSupplyHouse())
			{
				var test_subject = fools.build_me_a_city(_offer_to_raid_the_elves);
				test_subject.announce(new ElvesFound());
				test_subject.announce_and_wait(new SayGo(), TimeSpan.FromMilliseconds(100))
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
				var test_subject = fools.build_me_a_city(_offer_to_raid_the_elves);
				should_be_no_orcs();
				test_subject.announce(new ElvesFound());
				test_subject.announce_and_wait(new ElvesFound(), TimeSpan.FromMilliseconds(200))
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
			_orcs = new NonNullList<OrcishRaidProgress>();
			_all_orcs_are_sent = new ManualResetEventSlim(false);
			_target_number_of_orcs = 0;
		}

		[TearDown]
		public void tear_down()
		{
			_all_orcs_are_sent.Dispose();
			// ReSharper disable PossibleNullReferenceException
			_orcs.ForEach(o => o.Dispose());
			// ReSharper restore PossibleNullReferenceException
			_orcs.Clear();
		}

		private void _offer_to_raid_the_elves([NotNull] MissionLocation mission_location)
		{
			mission_location.send_out_fools_to(orc_raid());
		}

		private class OrcishRaidProgress : IDisposable
		{
			[NotNull] internal readonly ManualResetEventSlim went_raiding = new ManualResetEventSlim(false);

			// ReSharper disable UnusedParameter.Local
			public OrcishRaidProgress(int value_just_to_make_sure_lab_is_not_default_constructed_by_framework) {}
			// ReSharper restore UnusedParameter.Local

			public void Dispose()
			{
				went_raiding.Dispose();
			}

			public override string ToString()
			{
				return string.Format("went_raiding: {0}", went_raiding.IsSet);
			}
		}

		[NotNull] private NonNullList<OrcishRaidProgress> _orcs;
		[NotNull] private ManualResetEventSlim _all_orcs_are_sent;
		[NotNull] private readonly object _orc_counter = new object();
		private int _target_number_of_orcs;

		private void should_have_spawned_orcs(int count)
		{
			lock (_orc_counter)
			{
				_all_orcs_are_sent.Reset();
				_target_number_of_orcs = count;
				if (_target_number_of_orcs <= _orcs.Count) _all_orcs_are_sent.Set();
			}
			_all_orcs_are_sent.Wait(TimeSpan.FromMilliseconds(100))
				.Should()
				.BeTrue();
		}

		private void all_orcs_should_have_raided()
		{
			lock (_orc_counter)
			{
				_orcs.Should()
					.OnlyContain(o => o.went_raiding.IsSet);
			}
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

		private static void _begin_raiding([NotNull] OrcishRaidProgress lab, [NotNull] SayGo message)
		{
			lab.went_raiding.Set();
		}

		private void _start_new_raid([NotNull] OrcishRaidProgress lab, [NotNull] ElvesFound message)
		{
			lock (_orc_counter)
			{
				_orcs.Add(lab);
				if (_target_number_of_orcs > 0 && _orcs.Count >= _target_number_of_orcs) _all_orcs_are_sent.Set();
			}
		}

		private void should_be_no_orcs()
		{
			lock (_orc_counter)
			{
				_orcs.Count.Should()
					.Be(0);
			}
		}

		public void Dispose()
		{
			_all_orcs_are_sent.Dispose();
		}
	}

	internal class SayGo : MailMessage {}

	internal class ElvesFound : MailMessage {}
}
