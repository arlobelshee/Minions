using NUnit.Framework;

namespace Fools.cs.Tests.Interpretation
{
	[TestFixture]
	public class ExecuteContinuationsWithoutCallsAndObserveResults
	{
		[Test]
		public void should_write_a_simple_string_to_a_file()
		{
			using(var test_subject = new Interpreter())
			{
				Universe universe = test_subject.fools.create_universe();
				test_subject.fools.create_fool_in(universe);
			}
		}
	}
}
