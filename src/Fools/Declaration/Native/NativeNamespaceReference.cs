using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci;

namespace Fools.Declaration.Native
{
	internal class NativeNamespaceReference : Namespace
	{
		private readonly NativeCompiler _compiler;
		private readonly INamespaceDefinition _target;
		private readonly Dictionary<string, Definition> _members = new Dictionary<string, Definition>();

		public NativeNamespaceReference(NativeCompiler compiler, INamespaceDefinition target)
		{
			_compiler = compiler;
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }

		public override FrameDefinition get_continuation_definition(string type_name)
		{
			return (FrameDefinition) _cache(type_name, t => new NativeUserDefinedTypeReference(this, t));
		}

		public override FrameDefinition ensure_continuation_definition_exists(string type_name)
		{
			throw new InvalidOperationException(
				string.Format("{0} is a read-only namespace. You cannot add the function {1} to it.", name, type_name));
		}

		public override UserDefinedType get_udt(string type_name)
		{
			return (UserDefinedType) _cache(type_name, t => new NativeUserDefinedTypeReference(this, t));
		}

		public override UserDefinedType ensure_udt_exists(string type_name)
		{
			throw new InvalidOperationException(
				string.Format("{0} is a read-only namespace. You cannot add the type {1} to it.", name, type_name));
		}

		private Definition _cache(string type_name, Func<INamespaceMember, Definition> wrap)
		{
			Definition result;
			if (_members.TryGetValue(type_name, out result)) return result;
			return _members[type_name] = wrap(_target.GetMembersNamed(_compiler.name(type_name), false).Single());
		}
	}
}
