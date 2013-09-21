// MissionActivity.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public abstract class MissionActivity<TLab> : IEquatable<MissionActivity<TLab>> where TLab : class
	{
		[NotNull]
		public abstract Type message_type { get; }

		public void execute([NotNull] TLab lab, [NotNull] MailMessage message)
		{
			perform_action(lab, message);
		}

		protected abstract void perform_action([NotNull] TLab lab, [NotNull] MailMessage message);

		public abstract bool Equals([CanBeNull] MissionActivity<TLab> other);

		protected abstract int hash();

		public override bool Equals([CanBeNull] object obj)
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
