using Fools.Declaration;
using Fools.Tests.Support;
using NUnit.Framework;

namespace Fools.Tests.AssemblyModelCanExecuteOrEmitDotNetCode.BothInMemoryAndNativePlatformsCanModelDeclarations
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

		[Test]
		public void argument_sets_should_be_equality_comparable()
		{
			var foo = new Argument(TestData.arbitrary_type(), "argument_name");
			var bar = new Argument(TestData.different_type(), "__3");
			var baz = new Argument(TestData.arbitrary_type(), "baz");
			var test_subject = new ArgumentSet(foo, bar);
			var same_values = new ArgumentSet(foo, bar);
			var different_order = new ArgumentSet(bar, foo);
			var missing_one_arg = new ArgumentSet(foo);
			var one_different_arg = new ArgumentSet(foo, baz);

			Assert.AreEqual(test_subject, same_values);
			Assert.AreEqual(test_subject, different_order);
			Assert.AreNotEqual(test_subject, missing_one_arg);
			Assert.AreNotEqual(test_subject, one_different_arg);
		}
	}
}
