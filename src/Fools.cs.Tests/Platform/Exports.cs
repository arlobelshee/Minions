// Exports.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using FluentAssertions;
using Fools.cs.Api;
using Fools.cs.messages;
using NUnit.Framework;

namespace Fools.cs.Tests.Platform
{
	[TestFixture]
	public class Exports
	{
		[Test]
		public void should_export_all_the_right_messge_types()
		{
			var exports = Exported.messages();
			exports.Should()
				.Contain(Exported.message("write.to.log", typeof (WriteToLog)));
		}

		[Test]
		public void should_export_all_the_right_functions()
		{
			var exports = Exported.functions();
			exports.Should()
				.Contain(Exported.function("write.normal", () => WriteToLog.normal()));
		}
	}
}
