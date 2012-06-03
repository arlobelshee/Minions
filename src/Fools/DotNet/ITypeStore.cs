using System.Collections.Generic;

namespace Fools.DotNet
{
	public interface ITypeStore
	{
		string file_name { get; }
		string name { get; }
		IEnumerable<ITypeStore> references { get; }
	}
}