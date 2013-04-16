using System;
using FluentAssertions;
using NUnit.Framework;

namespace Fools.cs.Tests.Interpretation
{
	[TestFixture]
	public class ExecuteContinuationsWithoutCallsAndObserveResults
	{
		[Test]
		public void should_find_object_and_send_it_in_a_message()
		{
			using(var test_subject = new Interpreter())
			{
				Building skating_rink = test_subject.fools.create_building();
				skating_rink.create_string("sign", "Go skating now!");
				Mission read_the_sign = test_subject.fools.define_mission(
					"read the sign",
					new BuildingAccess[0],
					Mission.completes.when_out_of_assignments(),
					ResourceBudget.single_minion(),
					Cleanup.none());
				read_the_sign.add_assignment(new[] {skating_rink.window_access}, @"curent_test.send(""sign.said"", sign)");
				test_subject.execute();
				read_the_sign.Should().finish_in(TimeSpan.FromMilliseconds(20));
				test_subject.current_test.Should().have_gotten_message("sign.said", "Go skating now!");
				test_subject.resources_used.max_minions.Should().Be(1);
			}
		}
	}

	public class Cleanup
	{
		public static Cleanup none()
		{
			return new Cleanup();
		}
	}

	public class ResourceBudget
	{
		public static ResourceBudget single_minion()
		{
			throw new NotImplementedException();
		}
	}

	public class Mission
	{
		public static CompletionCriterionBuilder completes;

		public class CompletionCriterionBuilder
		{
			public CompletionCriterion when_out_of_assignments()
			{
				throw new NotImplementedException();
			}
		}

		public class CompletionCriterion
		{
		}

		public void add_assignment(BuildingAccess[] assignment_map_supplement, string assignment_continuation_fools_code)
		{
			throw new NotImplementedException();
		}
	}

	public class BuildingAccess
	{
		public Building building { get; set; }

		protected BuildingAccess(Building building)
		{
			this.building = building;
		}
	}
}
