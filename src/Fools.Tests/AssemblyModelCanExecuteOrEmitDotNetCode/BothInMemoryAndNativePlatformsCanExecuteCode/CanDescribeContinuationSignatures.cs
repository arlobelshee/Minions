using Fools.DotNet;
using Fools.Tests.Support;
using NUnit.Framework;
using FluentAssertions;

namespace Fools.Tests.AssemblyModelCanExecuteOrEmitDotNetCode.BothInMemoryAndNativePlatformsCanExecuteCode
{
	[TestFixture]
	public class CanDescribeContinuationSignatures
	{
		[Test]
		public void arguments_should_be_equality_comparable()
		{
			var test_subject = new Argument(TestData.arbitrary_type(), "argument_name");
			var same_values = new Argument(TestData.arbitrary_type(), "argument_name");
			var different_name = new Argument(TestData.arbitrary_type(), "completely_different_name");
			var different_type = new Argument(TestData.different_type(), "argument_name");
			Assert.AreEqual(test_subject, same_values);
			Assert.AreNotEqual(test_subject, different_name);
			Assert.AreNotEqual(test_subject, different_type);
		}
	}
}
