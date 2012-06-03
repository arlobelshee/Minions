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
		public override TypeDefinition get_type(string type_name)
		{
			return new NativeTypeDefinitionReference(this, _target.GetMembersNamed(_compiler.name(type_name), false).Single());
		}
	}
}