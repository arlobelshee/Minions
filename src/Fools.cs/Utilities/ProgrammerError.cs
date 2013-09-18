// ProgrammerError.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;

namespace Fools.cs.Utilities
{
	[PublicAPI]
	public class ProgrammerError : Exception
	{
		private ProgrammerError([NotNull] string message_format, [NotNull] object[] format_data)
			: base("Programmer error! "+string.Format(message_format, format_data)) {}

		[StringFormatMethod("message_format"), ContractAnnotation("condition:false =>halt")]
		// ReSharper disable UnusedParameter.Local
		public static void report_if(bool condition,
			// ReSharper restore UnusedParameter.Local
			[NotNull] string message_format,
			[NotNull] params object[] format_data)
		{
			if (!condition) throw new ProgrammerError(message_format, format_data);
		}
	}
}
