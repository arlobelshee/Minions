// ConstructionSite.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class ConstructionSite
	{
		[NotNull] private readonly HashSet<Type> _valid_messages = new HashSet<Type>();
		private bool _built;

		protected ConstructionSite([NotNull] string name, [NotNull] CityMap city_map)
		{
			this.name = name;
			this.city_map = city_map;
		}

		[NotNull]
		public string name { get; private set; }

		[NotNull]
		public CityMap city_map { get; private set; }

		[NotNull]
		public ConstructionSite will_pass_message<TMessage>() where TMessage : MailMessage
		{
			return will_pass_message(typeof (TMessage));
		}

		[NotNull]
		public ConstructionSite will_pass_message([NotNull] Type valid_message)
		{
			if (_built)
			{
				throw new InvalidOperationException(
					String.Format("Illegal attempt to add new message type after creating mail room. Attempted to add {0} to {1}.",
						valid_message.Name,
						name));
			}
			_valid_messages.Add(valid_message);
			return this;
		}

		[NotNull]
		public static ConstructionSite undisclosed_location([NotNull] string purpose, [NotNull] CityMap city_map)
		{
			return new ConstructionSite(String.Format("an undisclosed location for {0}", purpose), city_map);
		}

		[NotNull]
		public static ConstructionSite public_building([NotNull] string name, [NotNull] CityMap city_map)
		{
			return new PublicBuilding(name, city_map);
		}

		[NotNull]
		public virtual MailRoom create_dead_drop([NotNull] FoolFactory fool_factory, [NotNull] params object[] name_args)
		{
			_built = true;
			return new MailRoom(new MailIndex(_valid_messages, String.Format(name, name_args)), fool_factory, city_map);
		}
	}

	internal class PublicBuilding : ConstructionSite
	{
		[CanBeNull] private MailRoom _building;

		public PublicBuilding([NotNull] string name, [NotNull] CityMap city_map) : base(name, city_map) {}

		public override MailRoom create_dead_drop(FoolFactory fool_factory, params object[] name_args)
		{
			return _building ?? (_building = base.create_dead_drop(fool_factory, name_args));
		}
	}
}
