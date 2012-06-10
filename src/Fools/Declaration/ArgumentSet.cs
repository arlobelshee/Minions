using System;
using System.Collections.Generic;
using Fools.Utils;

namespace Fools.Declaration
{
	public class ArgumentSet : IEquatable<ArgumentSet>
	{
		public readonly HashSet<Argument> args = new HashSet<Argument>();

		public ArgumentSet(params Argument[] args)
		{
			args.Each(a => this.args.Add(a));
		}

		public bool Equals(ArgumentSet other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return args.SetEquals(other.args);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ArgumentSet);
		}

		public override int GetHashCode()
		{
			return args.GetHashCode();
		}

		public static bool operator ==(ArgumentSet left, ArgumentSet right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ArgumentSet left, ArgumentSet right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return string.Format("{{{0}}}", string.Join(", ", args));
		}
	}
}