using System;
using System.Collections.Generic;
using System.Linq;

namespace Fools.Model.Static
{
	public class StackFrame : IEquatable<StackFrame>
	{
		private readonly Type[] _variable_types;

		public StackFrame(params Type[] variable_types)
		{
			_variable_types = variable_types;
		}

		public StackFrame(IEnumerable<Type> variable_types)
		{
			_variable_types = variable_types.ToArray();
		}

		public bool Equals(StackFrame other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return other._variable_types.SequenceEqual(_variable_types);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as StackFrame);
		}

		public override int GetHashCode()
		{
			return _variable_types.Select(v => v.GetHashCode()).Aggregate((c, n) => (c*397) ^ n);
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
			return string.Format("Frame[{0}]", string.Join(", ", _variable_types.Select(v => v.Name)));
		}
	}
}