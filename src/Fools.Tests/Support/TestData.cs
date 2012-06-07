namespace Fools.Tests.Support
{
	static internal class TestData
	{
		public static TestUdt arbitrary_type()
		{
			return new TestUdt("type.name");
		}

		public static TestUdt different_type()
		{
			return new TestUdt("type.second.name");
		}
	}
}