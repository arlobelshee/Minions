﻿using System.Linq;
using FluentAssertions;
using Fools.DotNet;
using Fools.DotNet.Native;
using Microsoft.Cci;
using NUnit.Framework;

namespace Fools.Tests.AssemblyModelCanExecuteOrEmitDotNetCode.BothInMemoryAndNativePlatformsCanExecuteCode
{
	[TestFixture]
	public class CanMakeAPlaceToMakeTypes
	{
		private const string _DEFAULT_NAMESPACE = "Fools.TestNamespace";

		[Test]
		public void library_should_determine_basics_from_default_namespace()
		{
			var library = make_compiler().new_library(_DEFAULT_NAMESPACE);
			library.file_name.Should().Be(_DEFAULT_NAMESPACE + ".dll");
			library.references.Select(r => r.name).Should().Equal(
				new object[]
				{
					"mscorlib"
				});
			library.default_namespace.name.Should().Be(_DEFAULT_NAMESPACE);
		}

		[Test]
		public void library_should_allow_adding_namespaces()
		{
			var library = make_compiler().new_library(_DEFAULT_NAMESPACE);
			var ns = library.ensure_namespace_exists(_DEFAULT_NAMESPACE + ".subnamespace");
			ns.name.Should().Be(_DEFAULT_NAMESPACE + ".subnamespace");
			library.ensure_namespace_exists(_DEFAULT_NAMESPACE + ".subnamespace").Should().BeSameAs(ns);
			// next up: ensure that we can ask the base namespace for this namespace. Should that return the hierarchical child or not?
		}

		protected virtual Compiler make_compiler()
		{
			return DotNetCode.simulated();
		}
	}

	[Explicit]
	public class CanMakeAPlaceToMakeTypesNative : CanMakeAPlaceToMakeTypes
	{
		[Test]
		public void native_library_should_create_type_for_module()
		{
			var compiler = new NativeCompiler();
			var test_subject = new NativeAssembly(compiler, "irrelevant", ModuleKind.DynamicallyLinkedLibrary);
			var module_description = test_subject.get_type("<Module>");
			module_description.name.Should().Be("<Module>");
			module_description.name_space.name.Should().Be(string.Empty);
		}

		protected override Compiler make_compiler()
		{
			return DotNetCode.native();
		}
	}
}
