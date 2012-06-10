using System;
using Fools.Utils;

namespace Fools.Declaration
{
	public class Argument : IEquatable<Argument>
	{
		public readonly UserDefinedType type;
		public readonly string name;

		public Argument(UserDefinedType type, string name)
		{
			Require.that(type != null, () => new ArgumentNullException("type"));
			Require.that(name != null, () => new ArgumentNullException("name"));
			this.type = type;
			this.name = name;
		}

		public bool Equals(Argument other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return Equals(other.type, type) && Equals(other.name, name);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Argument);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (type.GetHashCode()*397) ^ name.GetHashCode();
			}
		}

		public static bool operator ==(Argument left, Argument right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Argument left, Argument right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", type, name);
		}
	}
}
