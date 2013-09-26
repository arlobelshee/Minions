// OverlordThrone.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Threading;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	[PublicAPI]
	public class OverlordThrone : IDisposable
	{
		private AppErrorLevel _result = AppErrorLevel.Ok;
		[NotNull] private readonly ManualResetEventSlim _program_complete = new ManualResetEventSlim(false);

		public AppErrorLevel while_I_wait_for_the_world_to_end()
		{
			_program_complete.Wait();
			return _result;
		}

		public void show_him_what_you_do([NotNull] FoolSupplyHouse mission_control)
		{
			var tell_overlord_when_all_work_is_done = NewMission.in_lab(() => this);
			tell_overlord_when_all_work_is_done.send_new_fool_when<DoMyBidding>();
			tell_overlord_when_all_work_is_done.whenever<AppQuit>(stop_program);
			mission_control.send_out_fools_to(tell_overlord_when_all_work_is_done);
		}

		private static void stop_program([NotNull] OverlordThrone lab, [NotNull] AppQuit message)
		{
			lab._result = message.result;
			lab._program_complete.Set();
		}

		public void Dispose()
		{
			if (!_program_complete.IsSet)
			{
				_result = AppErrorLevel.Unknown;
				_program_complete.Set();
			}
			_program_complete.Dispose();
		}
	}
}
