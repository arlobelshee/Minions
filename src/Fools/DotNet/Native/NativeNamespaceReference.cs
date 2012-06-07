using System;
using Microsoft.Cci;
using System.Linq;

namespace Fools.DotNet.Native
{
	public class NativeNamespaceReference : Namespace
	{
		private readonly NativeCompiler _compiler;
		private readonly INamespaceDefinition _target;

		public NativeNamespaceReference(NativeCompiler compiler, INamespaceDefinition target)
		{
			_compiler = compiler;
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }
		public override TypeDefinition get_continuation_definition(string type_name)
		{
			return new NativeTypeDefinitionReference(this, _target.GetMembersNamed(_compiler.name(type_name), false).Single());
		}

		public override TypeDefinition ensure_continuation_definition_exists(string type_name)
		{
			throw new InvalidOperationException(string.Format("{0} is a read-only namespace. You cannot add the type {1} to it.", name, type_name));
		}
	}
}