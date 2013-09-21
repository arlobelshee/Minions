// MissionVerificationBuilder.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using Fools.cs.Api;
using Fools.cs.Utilities;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;

namespace core_compile.tests.InteractWithUser
{
	internal class MissionVerificationBuilder<TLab> where TLab : class
	{
		[NotNull] private readonly NonNullList<MissionActivity<TLab>> _activities = new NonNullList<MissionActivity<TLab>>();
		[NotNull] private readonly NonNullList<Type> _spawning_messages = new NonNullList<Type>();

		[NotNull]
		public MissionVerificationBuilder<TLab> spawn_per<TMessage>()
		{
			_spawning_messages.Add(typeof (TMessage));
			return this;
		}

		[NotNull]
		public MissionVerificationBuilder<TLab> when<TMessage>([NotNull] Action<TLab, TMessage> do_what) where TMessage : MailMessage
		{
			_activities.Add(new MissionActivityTypeSpecific<TLab, TMessage>(do_what));
			return this;
		}

		[NotNull]
		public MissionDescription<TLab> build()
		{
			var matcher = new MissionDescriptionMatcher<TLab>(_spawning_messages, _activities);
			return _make_arg_spec(matcher);
		}

		[NotNull]
		private static MissionDescription<TLab> _make_arg_spec(IArgumentMatcher matcher)
		{
			var spec_builder = new ArgumentSpecificationQueue(SubstitutionContext.Current);
			return _pretend_is_non_null(spec_builder.EnqueueSpecFor<MissionDescription<TLab>>(matcher));
		}

		[NotNull]
		private static T _pretend_is_non_null<T>(T input)
		{
			// ReSharper disable AssignNullToNotNullAttribute
			return input;
			// ReSharper restore AssignNullToNotNullAttribute
		}
	}
}
