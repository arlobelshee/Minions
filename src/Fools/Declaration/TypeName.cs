using System;

namespace Fools.Declaration
{
	public class TypeName
	{
		public string namespace_name { get; private set; }
		public string type_name { get; private set; }

		private TypeName(string namespace_name, string type_name)
		{
			this.namespace_name = namespace_name;
			this.type_name = type_name;
		}

		public static TypeName of(string full_name)
		{
			var end_of_namespace_name = Math.Max(0, full_name.LastIndexOf('.'));
			return new TypeName(full_name.Substring(0, end_of_namespace_name), full_name.Substring(end_of_namespace_name));
		}
	}
}