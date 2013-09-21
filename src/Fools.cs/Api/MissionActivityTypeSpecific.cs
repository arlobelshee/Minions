// MissionActivityTypeSpecific.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class MissionActivityTypeSpecific<TLab, TMessage> : MissionActivity<TLab> where TLab : class
		where TMessage : MailMessage
	{
		[NotNull] private readonly Action<TLab, TMessage> _message_response;

		public MissionActivityTypeSpecific([NotNull] Action<TLab, TMessage> message_response)
		{
			_message_response = message_response;
		}

		protected override void perform_action(TLab lab, MailMessage message)
		{
			_message_response(lab, (TMessage) message);
		}
	}
}
