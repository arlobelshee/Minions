// MissionActivity.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;

namespace Fools.cs.Api
{
	public abstract class MissionActivity<TLab> : IEquatable<MissionActivity<TLab>> where TLab : class
	{
		public void execute(TLab lab, MailMessage message)
		{
			perform_action(lab, message);
		}

		protected abstract void perform_action(TLab lab, MailMessage message);
		public abstract bool Equals(MissionActivity<TLab> other);
		protected abstract int hash();

		public override bool Equals(object obj)
		{
			return Equals(obj as MissionActivity<TLab>);
		}

		public override int GetHashCode()
		{
			return hash();
		}

		public static bool operator ==(MissionActivity<TLab> left, MissionActivity<TLab> right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(MissionActivity<TLab> left, MissionActivity<TLab> right)
		{
			return !Equals(left, right);
		}
	}
}
