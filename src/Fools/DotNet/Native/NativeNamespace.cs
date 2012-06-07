using System.Collections.Generic;
using Microsoft.Cci.MutableCodeModel;

namespace Fools.DotNet.Native
{
	public class NativeNamespace : Namespace
	{
		private readonly NativeCompiler _compiler;
		private readonly Assembly _assembly;
		private readonly UnitNamespace _target;
		private readonly Dictionary<string, FrameDefinition> _members = new Dictionary<string, FrameDefinition>();

		public NativeNamespace(NativeCompiler compiler, Assembly assembly, UnitNamespace target)
		{
			_compiler = compiler;
			_assembly = assembly;
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }

		public override FrameDefinition get_continuation_definition(string type_name)
		{
			return _members[type_name];
		}

		public override FrameDefinition ensure_continuation_definition_exists(string type_name)
		{
			FrameDefinition result;
			return _members.TryGetValue(type_name, out result) ? result : _remember_type(_create_class(type_name));
		}

		private NamespaceTypeDefinition _create_class(string class_name)
		{
			var new_type = new NamespaceTypeDefinition
			               {
			               	ContainingUnitNamespace = _target,
			               	InternFactory = _compiler.intern_factory,
			               	IsClass = true,
			               	Name = _compiler.name(class_name)
			               };
			_target.Members.Add(new_type);
			_assembly.AllTypes.Add(new_type);
			return new_type;
		}

		private FrameDefinition _remember_type(NamespaceTypeDefinition target)
		{
			var result = new NativeFrameDefinition(this, target);
			return _members[result.name] = result;
		}

		public void create_raw_type(string type_name)
		{
			_remember_type(_create_class(type_name));
		}
	}
}
