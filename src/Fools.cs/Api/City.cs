// City.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class City : IDisposable
	{
		[NotNull] public readonly FoolFactory fools = FoolFactory.using_background_threads();
		[NotNull] public readonly CityMap map = new CityMap();
		public void Dispose() {}
	}
}
