using System.Collections.Generic;
using Microsoft.Cci.MutableCodeModel;

namespace Fools.DotNet.Native
{
	public class NativeNamespace : Namespace
	{
		private readonly NativeCompiler _compiler;
		private readonly Assembly _assembly;
		private readonly UnitNamespace _target;
		private readonly Dictionary<string, TypeDefinition> _members = new Dictionary<string, TypeDefinition>();

		public NativeNamespace(NativeCompiler compiler, Assembly assembly, UnitNamespace target)
		{
			_compiler = compiler;
			_assembly = assembly;
			_target = target;
		}

		public override string name { get { return _target.Name.Value; } }

		public override TypeDefinition get_type(string type_name)
		{
			return _members[type_name];
		}

		public TypeDefinition create_class(string class_name)
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
			return _remember_type(new_type);
		}

		private TypeDefinition _remember_type(NamespaceTypeDefinition target)
		{
			var result = new NativeTypeDefinition(this, target);
			_members.Add(result.name, result);
			return result;
		}
	}
}
