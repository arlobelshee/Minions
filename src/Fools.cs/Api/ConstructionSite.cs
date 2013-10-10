// ConstructionSite.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;

namespace Fools.cs.Api
{
	public class ConstructionSite<TLab> where TLab : class
	{
		private readonly MissionDescription<TLab> _mission_description;

		public ConstructionSite(MissionDescription<TLab> mission_description)
		{
			_mission_description = mission_description;
		}

		public void when_built(Action<TLab, DeadDrop> build_notification) {}
	}
}
