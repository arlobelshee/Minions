// ConstructionSite.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public abstract class ConstructionSite
	{
		[NotNull] private readonly HashSet<Type> _valid_messages = new HashSet<Type>();
		private bool _built;

		protected ConstructionSite([NotNull] string name)
		{
			this.name = name;
		}

		[NotNull]
		public string name { get; private set; }

		[NotNull]
		public abstract MailRoom create_dead_drop();

		public void will_pass_message<TMessage>() where TMessage : MailMessage
		{
			will_pass_message(typeof (TMessage));
		}

		public void will_pass_message([NotNull] Type valid_message)
		{
			if (_built)
			{
				throw new InvalidOperationException(
					string.Format("Illegal attempt to add new message type after creating mail room. Attempted to add {0} to {1}.",
						valid_message.Name,
						name));
			}
			_valid_messages.Add(valid_message);
		}

		[NotNull]
		protected MailRoom build_new_room()
		{
			_built = true;
			return new MailRoom(_valid_messages, name);
		}
	}

	internal class PublicBuilding : ConstructionSite
	{
		[CanBeNull] private MailRoom _building;

		private PublicBuilding([NotNull] string name) : base(name) {}

		public static ConstructionSite named([NotNull] string name)
		{
			return new PublicBuilding(name);
		}

		public override MailRoom create_dead_drop()
		{
			return _building ?? (_building = build_new_room());
		}
	}

	internal class UndisclosedLocation : ConstructionSite
	{
		private UndisclosedLocation([NotNull] string name) : base(name) {}

		[NotNull]
		public static ConstructionSite to_do([NotNull] string purpose)
		{
			return new UndisclosedLocation(String.Format("an undisclosed location for {0}", purpose));
		}

		public override MailRoom create_dead_drop()
		{
			return build_new_room();
		}
	}
}
