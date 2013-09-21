// NewMission.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public static class NewMission
	{
		[NotNull]
		public static MissionDescription<TLab> in_lab<TLab>([NotNull] Func<TLab> lab_constructor) where TLab : class
		{
			return new MissionDescription<TLab>(lab_constructor);
		}
	}
}
