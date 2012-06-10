using System.Collections.Generic;
using Microsoft.Cci.MutableCodeModel;

namespace Fools.Declaration.Native
{
	internal class NativeNamespace : Namespace
	{
		private readonly NativeCompiler _compiler;
		private readonly Assembly _assembly;
		private readonly UnitNamespace _target;
		private readonly Dictionary<string, Definition> _members = new Dictionary<string, Definition>();

		public NativeNamespace(NativeCompiler compiler, Assembly assembly, UnitNamespace target)
		{
			_compiler = compiler;
			_assembly = assembly;
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }

		public override FrameDefinition get_continuation_definition(string type_name)
		{
			return (FrameDefinition) _members[type_name];
		}

		public override FrameDefinition ensure_continuation_definition_exists(string type_name)
		{
			Definition result;
			return _members.TryGetValue(type_name, out result) ? (FrameDefinition) result : _remember_frame(_create_class(type_name));
		}

		public override UserDefinedType get_udt(string type_name)
		{
			return (UserDefinedType) _members[type_name];
		}

		public override UserDefinedType ensure_udt_exists(string type_name)
		{
			Definition result;
			return _members.TryGetValue(type_name, out result) ? (UserDefinedType) result : _remember_udt(_create_class(type_name));
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

		private FrameDefinition _remember_frame(NamespaceTypeDefinition target)
		{
			var result = new NativeFrameDefinition(this, target);
			return (FrameDefinition) (_members[result.name] = result);
		}

		private UserDefinedType _remember_udt(NamespaceTypeDefinition target)
		{
			var result = new NativeUserDefinedType(this, target);
			return (UserDefinedType) (_members[result.name] = result);
		}

		public void create_raw_type(string type_name)
		{
			_remember_frame(_create_class(type_name));
		}
	}
}
