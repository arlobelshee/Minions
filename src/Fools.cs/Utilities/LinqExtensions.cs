// LinqExtensions.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System.Collections.Generic;
using Fools.cs.Utilities;

// ReSharper disable CheckNamespace

namespace System.Linq // ReSharper restore CheckNamespace
{
	public static class LinqExtensions
	{
		public static void each<T>([NotNull] this IEnumerable<T> items, [NotNull] Action<T> op)
		{
			foreach (var item in items)
			{
				op(item);
			}
		}

		public static void if_valid<T>([CanBeNull] this T arg, [NotNull] Action<T> op) where T : class
		{
			if (null != arg) op(arg);
		}

		public static TResult if_valid<TArg, TResult>([CanBeNull] this TArg arg, [NotNull] Func<TArg, TResult> op)
			where TArg : class
		{
			return null == arg ? default(TResult) : op(arg);
		}

		public static bool sequence_equivalent<T>([NotNull] this IEnumerable<T> lhs, [NotNull] IEnumerable<T> rhs)
		{
			return lhs.mismatches_with(rhs)
				.is_empty;
		}

		[NotNull]
		public static Mismatches<T> mismatches_with<T>([NotNull] this IEnumerable<T> lhs, [NotNull] IEnumerable<T> rhs)
		{
			var left = lhs.GetEnumerator();
			var missing_from_right = new List<T>();
			var not_yet_found_in_left = rhs.ToList();
			while (left.MoveNext())
			{
				var first_match = not_yet_found_in_left.IndexOf(left.Current);
				if (first_match < 0) missing_from_right.Add(left.Current);
				else not_yet_found_in_left.RemoveAt(first_match);
			}
			var mismatches = new Mismatches<T>(missing_from_right, not_yet_found_in_left);
			return mismatches;
		}
	}
}
