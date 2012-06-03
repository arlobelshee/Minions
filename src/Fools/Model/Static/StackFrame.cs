using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Fools.Model.Static
{
	public class StackFrame : IEquatable<StackFrame>
	{
		private readonly Type[] _argument_types;
		private readonly Type[] _return_types;
		private readonly Type[] _variable_types;

		public StackFrame(IEnumerable<Type> variable_types, Type[] argument_types, Type[] return_types)
		{
			_argument_types = argument_types;
			_return_types = return_types;
			_variable_types = variable_types.ToArray();
		}

		public bool Equals(StackFrame other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return other._variable_types.SequenceEqual(_variable_types) && other._argument_types.SequenceEqual(_argument_types) &&
				other._return_types.SequenceEqual(_return_types);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as StackFrame);
		}

		public override int GetHashCode()
		{
			return (((_variable_types.Select(v => v.GetHashCode()).Aggregate((c, n) => (c*397) ^ n)*397) ^
				(_argument_types.Select(v => v.GetHashCode()).Aggregate((c, n) => (c*397) ^ n)*397))*397) ^
					(_return_types.Select(v => v.GetHashCode()).Aggregate((c, n) => (c*397) ^ n)*397);
		}

		public static bool operator ==(StackFrame left, StackFrame right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(StackFrame left, StackFrame right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return string.Format(
				"{{Frame locals:[{0}] params:[{1}] returns:[{2}]}}",
				string.Join(", ", _variable_types.Select(v => v.Name)),
				string.Join(", ", _argument_types.Select(v => v.Name)),
				string.Join(", ", _return_types.Select(v => v.Name)));
		}

		public Type emit()
		{
			return typeof(int);
		}
	}
}
