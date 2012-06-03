using System.Collections.Generic;
using Microsoft.Cci;

namespace Fools.DotNet.Native
{
	public class NativeAssemblyReference : ITypeStore
	{
		private readonly IAssemblyReference _target;

		public NativeAssemblyReference(IAssemblyReference target)
		{
			_target = target;
		}

		public string file_name { get { return _target.Name.Value; } }

		public string name { get { return _target.Name.Value; } }

		public IEnumerable<ITypeStore> references { get { return Enumerable<ITypeStore>.Empty; } }
	}
}