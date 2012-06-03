using System.Reflection;
using System.Reflection.Emit;
using Fools.Model.Execution;

namespace Fools.Model.Static
{
	public class Function
	{
		private readonly string _name;

		public Function(string name, StackFrame frame)
		{
			_name = name;
			this.frame = frame;
		}

		public StackFrame frame { get; private set; }

		public DynamicMethod emit()
		{
			var result = new DynamicMethod(
				_name,
				MethodAttributes.Static | MethodAttributes.Public,
				CallingConventions.Standard,
				typeof(FunctionCall),
				new[]
				{
					typeof(StackFrame), typeof(VariableGroup), typeof(VariableGroup), typeof(FunctionCall),
				},
				typeof(DynamicMethod).Module,
				false);
			result.DefineParameter(1, ParameterAttributes.None, "locals");
			result.DefineParameter(2, ParameterAttributes.None, "args");
			result.DefineParameter(3, ParameterAttributes.None, "return_bindings");
			result.DefineParameter(4, ParameterAttributes.None, "return_continuation");
			return result;
		}
	}
}
