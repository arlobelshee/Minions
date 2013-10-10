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
		public void anonyous_locations_should_know_their_purpose()
		{
			var first = _map.secret_location("watching the game");
			first.name.Should()
				.Be("an undisclosed location for watching the game");
		}

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
			const string arbitrary_purpose = "watching the game";
			var first = _map.secret_location(arbitrary_purpose);
			var second = _map.secret_location(arbitrary_purpose);
			first.Should()
				.NotBeSameAs(second);
		}

		[Test]
		public void map_should_allow_finding_well_known_construction_sites_by_name()
		{
			var first = _map.public_location(PublicBuildings.APPLICATION);
			var second = _map.public_location(PublicBuildings.APPLICATION);
			first.Should()
				.BeSameAs(second);
		}

		[Test]
		public void well_known_sites_should_know_their_names()
		{
			var first = _map.public_location(PublicBuildings.APPLICATION);
			first.name.Should()
				.Be(PublicBuildings.APPLICATION);
		}

		[SetUp]
		public void set_up()
		{
			_map = new CityMap();
		}

		[NotNull] private CityMap _map;
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
			var site = _well_known_sites.GetOrAdd(building_name, name => new ConstructionSite(name));
			Debug.Assert(site != null, "Well-known sites should always exist.");
			return site;
		}

		[NotNull]
		public ConstructionSite secret_location([NotNull] string purpose)
		{
			return new ConstructionSite(string.Format("an undisclosed location for {0}", purpose));
		}
	}

	public class ConstructionSite
	{
		public ConstructionSite([NotNull] string name)
		{
			this.name = name;
		}

		[NotNull]
		public string name { get; private set; }
	}

	public class MissionControl {}
}
