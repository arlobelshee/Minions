// MissionLogToStream.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using Fools.cs.messages;

namespace Fools.cs.builtins.ProgramShells
{
	public class MissionLogToStream
	{
		public static IEnumerable<MessageSubscription> interesting_messages()
		{
			return new[] {MessageSubscription.to<Write__Normal>(send_message)};
		}

		public static void send_message(Write__Normal message)
		{
			throw new NotImplementedException();
		}
	}
}
