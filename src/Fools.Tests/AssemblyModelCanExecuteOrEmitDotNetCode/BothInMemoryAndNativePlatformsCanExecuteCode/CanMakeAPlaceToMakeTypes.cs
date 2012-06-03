using System.Linq;
using FluentAssertions;
using Fools.DotNet;
using NUnit.Framework;

namespace Fools.Tests.AssemblyModelCanExecuteOrEmitDotNetCode.BothInMemoryAndNativePlatformsCanExecuteCode
{
	[TestFixture]
	public class CanMakeAPlaceToMakeTypes
	{
		[Test]
		public void library_should_determine_basics_from_default_namespace()
		{
			var library = DotNetCode.native().new_library("Fools.TestNamespace");
			library.file_name.Should().Be("Fools.TestNamespace.dll");
			library.references.Select(r => r.name).Should().Equal(
				new object[]
				{
					"mscorlib"
				});
			library.default_namespace.name.Should().Be("Fools.TestNamespace");
		}
	}
}
