// ProcessCounter.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using FluentAssertions;
using Fools.cs.Utilities;
using NUnit.Framework;

namespace Fools.cs.Tests.ImplementationConcerns
{
	[TestFixture]
	public class ProcessCounter
	{
		[Test]
		public void should_never_fire_on_construction()
		{
			var fired = false;
			WaitableCounter.starting_at(0, ()=>fired=true);
			fired.Should()
				.BeFalse();
		}

		[Test]
		public void should_fire_when_counter_reaches_zero()
		{
			var fired = false;
			var test_subject = WaitableCounter.starting_at(1, () => fired = true);
			test_subject.done();
			fired.Should()
				.BeTrue();
		}

		[Test]
		public void counter_should_match_done_minus_begin_plus_initial()
		{
			var fired = false;
			var test_subject = WaitableCounter.starting_at(1, () => fired = true);
			test_subject.begin();
			test_subject.done();
			fired.Should()
				.BeFalse();
			test_subject.done();
			fired.Should()
				.BeTrue();
		}

		[Test]
		public void should_never_fire_twice()
		{
			var fired = false;
			var test_subject = WaitableCounter.starting_at(1, () => fired = true);
			test_subject.done();
			fired = false;
			test_subject.done();
			fired.Should()
				.BeFalse();
		}
	}
}
