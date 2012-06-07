using NUnit.Framework;

namespace Fools.Tests.AssemblyModelCanExecuteOrEmitDotNetCode.BothInMemoryAndNativePlatformsCanExecuteCode
{
	[TestFixture]
	public class CanDescribeContinuationSignatures
	{
		[Test]
		public void arguments_should_be_equality_comparable()
		{
			var test_subject = new ContinuationArguments();
		}
	}

	public class ContinuationArguments
	{
	}
}
