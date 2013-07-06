// MessageSubscriptionImpl.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Utilities;

namespace Fools.cs.builtins
{
	public class MessageSubscriptionImpl<T> : MessageSubscription
	{
		[NotNull] private readonly Action<T> _handler;
		[NotNull] private readonly string _key;

		public MessageSubscriptionImpl([NotNull] Action<T> handler)
		{
			_handler = handler;
			_key = typeof (T).Name;
		}

		protected override bool is_equal(MessageSubscription obj)
		{
			var other = obj as MessageSubscriptionImpl<T>;
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return _handler.Equals(other._handler);
		}

		public override int GetHashCode()
		{
			return _handler.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("whenever {2}: {0}.{1}", _handler.Method.DeclaringType.Name, _handler.Method.Name, key);
		}

		protected string key { get { return _key; } }
	}
}
