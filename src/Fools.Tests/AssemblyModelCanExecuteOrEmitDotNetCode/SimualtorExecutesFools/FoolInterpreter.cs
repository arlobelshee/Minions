using System.Collections.Generic;
using Fools.Declaration;
using Fools.Platform;
using Microsoft.Cci;

namespace Fools.Tests.AssemblyModelCanExecuteOrEmitDotNetCode.SimualtorExecutesFools
{
	public class FoolInterpreter
	{
		public CallableContinuation load(FrameDefinition continuation_declaration)
		{
			return null;
		}

		public IEnumerable<CallableContinuation> interpret(CallableContinuation call, IEnumerable<Universe> universes, ReadOnlyVoidFrame inputs, WriteOnlyRecordingVoidFrame outputs)
		{
			return Enumerable<CallableContinuation>.Empty;
		}
	}

	public class CallableContinuation
	{
	}
}