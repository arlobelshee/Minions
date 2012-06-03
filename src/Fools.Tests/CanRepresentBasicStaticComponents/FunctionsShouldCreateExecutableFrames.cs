using FluentAssertions;
using Fools.Model;
using Fools.Model.Static;
using NUnit.Framework;

namespace Fools.Tests.CanRepresentBasicStaticComponents
{
	[TestFixture]
	public class FunctionsShouldCreateExecutableFrames
	{
		[Test]
		public void function_body_should_support_local_variables()
		{
			var test_subject = new FunctionBuilder();
			test_subject.add_local(typeof(int)).add_local(typeof(string));
			test_subject.build().frame.Should().Be(new StackFrame(typeof(int), typeof(string)));
		}
	}
}
