// CreateTheCityMap.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using FluentAssertions;
using Fools.cs.Api;
using Fools.cs.Utilities;
using NUnit.Framework;

namespace Fools.cs.Tests.CoreLanguage
{
	[TestFixture]
	public class CreateTheCityMap
	{
		[Test]
		public void each_anonymous_location_should_be_distinct()
		{
			var first = _map.secret_location(_arbitrary_purpose);
			var second = _map.secret_location(_arbitrary_purpose);
			first.Should()
				.BeOfType<UndisclosedLocation>()
				.And.NotBeSameAs(second);
		}

		[Test]
		public void anonyous_locations_should_know_their_purpose()
		{
			var test_subject = _map.secret_location("watching the game");
			test_subject.name.Should()
				.Be("an undisclosed location for watching the game");
		}

		[Test]
		public void map_should_allow_finding_well_known_construction_sites_by_name()
		{
			var first = _map.public_location(PublicBuildings.APPLICATION);
			var second = _map.public_location(PublicBuildings.APPLICATION);
			first.Should()
				.BeOfType<PublicBuilding>()
				.And.BeSameAs(second);
		}

		[Test]
		public void well_known_sites_should_know_their_names()
		{
			var test_subject = _map.public_location(PublicBuildings.APPLICATION);
			test_subject.name.Should()
				.Be(PublicBuildings.APPLICATION);
		}

		[Test]
		public void well_known_sites_should_create_the_same_dead_drop_every_time_you_ask()
		{
			var test_subject = _map.public_location(PublicBuildings.APPLICATION);
			var first = test_subject.create_dead_drop();
			var second = test_subject.create_dead_drop();
			first.Should()
				.NotBeNull()
				.And.BeSameAs(second);
		}

		[Test]
		public void secret_locations_should_create_a_new_dead_drop_every_time_you_ask()
		{
			var test_subject = _map.secret_location(_arbitrary_purpose);
			var first = test_subject.create_dead_drop();
			var second = test_subject.create_dead_drop();
			first.Should()
				.NotBeNull()
				.And.NotBeSameAs(second);
		}

		[Test]
		public void secret_construction_sites_should_accumulate_messages_that_that_building_might_see()
		{
			var test_subject = _map.secret_location(_arbitrary_purpose);
			test_subject.will_pass_message<SillyMessage>();
			var drop = test_subject.create_dead_drop();
			drop.allowed_messages.Should()
				.Equal(new object[] {"SillyMessage"});
		}

		[Test]
		public void public_construction_sites_should_accumulate_messages_that_that_building_might_see()
		{
			var test_subject = _map.public_location(PublicBuildings.APPLICATION);
			test_subject.will_pass_message<SillyMessage>();
			var drop = test_subject.create_dead_drop();
			drop.allowed_messages.Should()
				.Equal(new object[] {"SillyMessage"});
		}

		[Test]
		public void attempting_to_add_new_message_after_building_room_should_fail()
		{
			var test_subject = _map.secret_location(_arbitrary_purpose);
			test_subject.create_dead_drop();
			Action late_message_add = test_subject.will_pass_message<SillyMessage>;
			late_message_add.ShouldThrow<InvalidOperationException>()
				.WithMessage(
					string.Format(
						"Illegal attempt to add new message type after creating mail room. Attempted to add SillyMessage to {0}.",
						test_subject.name));
		}

		[SetUp]
		public void set_up()
		{
			_map = new CityMap();
		}

		private const string _arbitrary_purpose = "watching the game";

		[NotNull] private CityMap _map;
	}
}
