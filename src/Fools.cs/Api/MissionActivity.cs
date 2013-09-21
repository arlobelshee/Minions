// MissionActivity.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

namespace Fools.cs.Api
{
	public abstract class MissionActivity<TLab> where TLab : class
	{
		public void execute(TLab lab, MailMessage message)
		{
			perform_action(lab, message);
		}

		protected abstract void perform_action(TLab lab, MailMessage message);
	}
}
