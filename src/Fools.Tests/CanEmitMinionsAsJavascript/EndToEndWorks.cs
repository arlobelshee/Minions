using System;
using System.IO;
using ApprovalTests;
using ApprovalTests.Core;
using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;
using Fools.Ast;
using Fools.Compilation;
using NUnit.Framework;
using Approvals = ApprovalTests.Approvals;

namespace Fools.Tests.CanEmitMinionsAsJavascript
{
	[TestFixture]
	public class EndToEndWorks
	{
		[Ignore, Test, UseReporter(typeof(TortoiseTextDiffReporter)), Category("Acceptance")]
		public void hello_world_transforms_correctly()
		{
			VerifyJavascript("hello_world");
		}

		private void VerifyJavascript(string filename)
		{
			var text = File.ReadAllText(PathUtilities.GetDirectoryForCaller() + filename + ".minion");
			var platform = new JsBackend();
			new MinionsCompiler(platform).Compile(text);
			Approvals.Verify(new ApprovalTextWriter(platform.Code, "js"), new JavascriptNamer(filename), Approvals.GetReporter());
		}
	}

	internal class JsBackend : Backend
	{
		public string Code { get; private set; }
	}

	internal class MinionsCompiler
	{
		private readonly Backend _backend;

		public MinionsCompiler(Backend backend)
		{
			_backend = backend;
		}

		public void Compile(string text)
		{
			var parser = new UnderstandFools(text);
			parser.Subscribe(_backend);
			parser.Go();
		}
	}

	internal class Backend : IObserver<INode>
	{
		public virtual void OnNext(INode value)
		{
		}

		public virtual void OnError(Exception error)
		{
		}

		public virtual void OnCompleted()
		{
		}
	}

	internal class JavascriptNamer : IApprovalNamer
	{
		public JavascriptNamer(string filename)
		{
			Name = filename;
		}

		public string SourcePath { get { return PathUtilities.GetDirectoryForCaller(); } }

		public string Name { get; private set; }
	}
}
