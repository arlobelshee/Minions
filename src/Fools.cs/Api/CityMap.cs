// CityMap.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System.Collections.Concurrent;
using System.Diagnostics;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class CityMap
	{
		[NotNull] private readonly ConcurrentDictionary<string, ConstructionSite> _well_known_sites =
			new ConcurrentDictionary<string, ConstructionSite>();

		[NotNull]
		public ConstructionSite public_location([NotNull] string building_name)
		{
			var site = _well_known_sites.GetOrAdd(building_name, name=> ConstructionSite.public_building(name, this));
			Debug.Assert(site != null, "Well-known sites should always exist.");
			return site;
		}

		[NotNull]
		public ConstructionSite secret_location([NotNull] string purpose)
		{
			return ConstructionSite.undisclosed_location(purpose, this);
		}
	}
}
