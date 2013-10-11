// DefineMissions.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using FluentAssertions;
using Fools.cs.Utilities;
using NUnit.Framework;

namespace Fools.cs.Tests.CoreLanguage
{
	[TestFixture]
	public class DefineMissions
	{
		[Test]
		public void city_should_expose_building_commission_and_mission_control()
		{
			using (var test_subject = new City())
			{
				test_subject.fools.Should()
					.BeOfType<MissionControl>();
				test_subject.map.Should()
					.BeOfType<CityMap>();
			}
		}

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
			var first = _map.secret_location("watching the game");
			first.name.Should()
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
			var first = _map.public_location(PublicBuildings.APPLICATION);
			first.name.Should()
				.Be(PublicBuildings.APPLICATION);
		}

		[Test]
		public void should_be_able_to_ask_mission_control_for_a_mission_definition()
		{
			
		}

		[SetUp]
		public void set_up()
		{
			_map = new CityMap();
		}

		private const string _arbitrary_purpose = "watching the game";

		[NotNull] private CityMap _map;
	}

	public class UndisclosedLocation : ConstructionSite
	{
		private UndisclosedLocation([NotNull] string name) : base(name) {}

		[NotNull]
		public static ConstructionSite to_do([NotNull] string purpose)
		{
			return new UndisclosedLocation(String.Format("an undisclosed location for {0}", purpose));
		}
	}

	public class PublicBuilding : ConstructionSite
	{
		private PublicBuilding([NotNull] string name) : base(name) {}

		public static ConstructionSite named([NotNull] string name)
		{
			return new PublicBuilding(name);
		}
	}

	public class City : IDisposable
	{
		[NotNull] public readonly MissionControl fools = new MissionControl();
		[NotNull] public readonly CityMap map = new CityMap();
		public void Dispose() {}
	}

	public class CityMap
	{
		[NotNull] private readonly ConcurrentDictionary<string, ConstructionSite> _well_known_sites =
			new ConcurrentDictionary<string, ConstructionSite>();

		[NotNull]
		public ConstructionSite public_location([NotNull] string building_name)
		{
			var site = _well_known_sites.GetOrAdd(building_name, PublicBuilding.named);
			Debug.Assert(site != null, "Well-known sites should always exist.");
			return site;
		}

		[NotNull]
		public ConstructionSite secret_location([NotNull] string purpose)
		{
			return UndisclosedLocation.to_do(purpose);
		}
	}

	public class ConstructionSite
	{
		protected ConstructionSite([NotNull] string name)
		{
			this.name = name;
		}

		[NotNull]
		public string name { get; private set; }
	}

	public class MissionControl {}
}
