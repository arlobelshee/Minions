using NUnit.Framework;

namespace Fools.Tests.AssemblyModelCanExecuteOrEmitDotNetCode.BothInMemoryAndNativePlatformsCanExecuteCode
{
	[TestFixture]
	public class CanDescribeContinuationSignatures
	{
		[Test]
		public void arguments_should_be_equality_comparable()
		{
			var test_subject = new Argument("type.name", "argument_name");
		}
	}

	public class Argument
	{
		public Argument(string type_name, string name)
		{
			
		}
	}
}
