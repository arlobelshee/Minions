using System;
using System.Collections.Generic;
using Fools.Model.Static;

namespace Fools.Model
{
	public class FunctionBuilder
	{
		private readonly List<Type> _locals = new List<Type>();

		public FunctionBuilder add_local(Type type)
		{
			_locals.Add(type);
			return this;
		}

		public Function build()
		{
			return new Function(new StackFrame(_locals));
		}
	}
}