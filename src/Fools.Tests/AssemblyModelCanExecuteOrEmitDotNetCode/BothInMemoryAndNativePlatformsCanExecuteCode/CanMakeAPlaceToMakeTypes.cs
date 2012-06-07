using System.Linq;
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
		private const string _ARBITRARY_NAME = "subnamespace";

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
			var test_subject = make_compiler().new_library(_DEFAULT_NAMESPACE);
			var ns = test_subject.ensure_namespace_exists(_DEFAULT_NAMESPACE + "." + _ARBITRARY_NAME);
			ns.name.Should().Be(_DEFAULT_NAMESPACE + "." + _ARBITRARY_NAME);
			test_subject.ensure_namespace_exists(_DEFAULT_NAMESPACE + "." + _ARBITRARY_NAME).Should().BeSameAs(ns);
		}

		[Test]
		public void namespaces_should_allow_creating_types()
		{
			var test_subject = make_compiler().new_library(_DEFAULT_NAMESPACE).default_namespace;
			var result = test_subject.ensure_type_exists(_ARBITRARY_NAME);
			result.name.Should().Be(_ARBITRARY_NAME);
			test_subject.get_type(_ARBITRARY_NAME).Should().BeSameAs(result);
			test_subject.ensure_type_exists(_ARBITRARY_NAME).Should().BeSameAs(result);
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
