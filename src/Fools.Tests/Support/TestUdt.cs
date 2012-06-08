using System;
using Fools.DotNet;

namespace Fools.Tests.Support
{
	public class TestUdt : UserDefinedType, IEquatable<TestUdt>
	{
		private readonly string _name;

		public TestUdt(string name)
		{
			_name = name;
		}

		public override string name { get { return _name; } }

		public override Namespace name_space { get { throw new NotImplementedException(); } }

		public override string ToString()
		{
			return string.Format("<Type {0}>", name);
		}

		public bool Equals(TestUdt other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return Equals(other._name, _name);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as TestUdt);
		}

		public override int GetHashCode()
		{
			return _name.GetHashCode();
		}

		public static bool operator ==(TestUdt left, TestUdt right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(TestUdt left, TestUdt right)
		{
			return !Equals(left, right);
		}
	}
}