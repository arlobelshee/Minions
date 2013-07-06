// MessageSubscription.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Utilities;

namespace Fools.cs.builtins
{
	public abstract class MessageSubscription : IEquatable<MessageSubscription>
	{
		public static MessageSubscriptionImpl<T> to<T>([NotNull] Action<T> handler)
		{
			return new MessageSubscriptionImpl<T>(handler);
		}

		protected abstract bool is_equal(MessageSubscription other);

		public bool Equals(MessageSubscription other)
		{
			return is_equal(other);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MessageSubscription);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(MessageSubscription left, MessageSubscription right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(MessageSubscription left, MessageSubscription right)
		{
			return !Equals(left, right);
		}
	}
}
