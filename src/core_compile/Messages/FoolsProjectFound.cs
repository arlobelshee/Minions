// FoolsProjectFound.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using Fools.cs.Api;
using JetBrains.Annotations;
using Simulated._Fs;

namespace core_compile.Messages
{
	public class FoolsProjectFound : MailMessage
	{
		[NotNull]
		public FsDirectory project_directory { get; set; }

		public FoolsProjectFound([NotNull] FsDirectory project_directory)
		{
			this.project_directory = project_directory;
		}
	}
}
