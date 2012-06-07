using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci;

namespace Fools.DotNet.Native
{
	public class NativeAssemblyReference : TypeStore
	{
		private readonly IAssemblyReference _target;
		private readonly NativeCompiler _compiler;

		public NativeAssemblyReference(NativeCompiler compiler, IAssemblyReference target)
		{
			_target = target;
			_compiler = compiler;
		}

		public override string file_name { get { return _target.Name.Value; } }
		public override string name { get { return _target.Name.Value; } }

		public override IEnumerable<TypeStore> references { get { return Enumerable<TypeStore>.Empty; } }
		public override Namespace default_namespace { get { return _namespace_for(_target.ResolvedModule.NamespaceRoot); } }

		public override Namespace ensure_namespace_exists(string ns_name)
		{
			throw new InvalidOperationException(string.Format("{0} is a read-only assembly. You cannot add the namespace {1} to it.", name, ns_name));
		}

		public override FrameDefinition get_type(TypeName full_name)
		{
			return
				_namespace_for(
					(INamespaceDefinition)
						_target.ResolvedModule.UnitNamespaceRoot.GetMembersNamed(_compiler.name(full_name.namespace_name), false).Single())
					.get_continuation_definition(full_name.type_name);
		}

		private NativeNamespaceReference _namespace_for(INamespaceDefinition ns)
		{
			return new NativeNamespaceReference(_compiler, ns);
		}
	}
}
