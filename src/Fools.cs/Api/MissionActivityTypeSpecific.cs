// MissionActivityTypeSpecific.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Linq;
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

		public override bool Equals(MissionActivity<TLab> other)
		{
			var other_activity = other as MissionActivityTypeSpecific<TLab, TMessage>;
			if (ReferenceEquals(null, other_activity)) return false;
			if (ReferenceEquals(this, other_activity)) return true;
			return _message_response.Equals(other_activity._message_response);
		}

		protected override int hash()
		{
			return _message_response.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("when {0}, do {1}", typeof (TMessage).Name, _message_response.Method.if_valid(m => m.Name));
		}
	}
}
