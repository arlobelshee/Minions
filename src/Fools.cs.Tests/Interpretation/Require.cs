using System;

namespace Fools.cs.Tests.Interpretation
{
	public class Require
	{
		public static void not_null(object value, string param_name)
		{
			if(value == null)
			{
				throw new ArgumentNullException(param_name);
			}
		}

		public static void is_true(bool condition, Func<Exception> create_error)
		{
			if(!condition)
			{
				throw create_error();
			}
		}
	}
}