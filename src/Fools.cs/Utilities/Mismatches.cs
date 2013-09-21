// Mismatches.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System.Collections.Generic;
using Fools.cs.Utilities;

namespace System.Linq
{
	public class Mismatches<T>
	{
		[NotNull]
		public List<T> in_left_but_not_right { get; private set; }

		[NotNull]
		public List<T> in_right_but_not_left { get; private set; }

		public bool is_empty { get { return in_left_but_not_right.Count == 0 && in_right_but_not_left.Count == 0; } }

		public Mismatches([NotNull] List<T> in_left_but_not_right, [NotNull] List<T> in_right_but_not_left)
		{
			this.in_left_but_not_right = in_left_but_not_right;
			this.in_right_but_not_left = in_right_but_not_left;
		}

		[NotNull]
		public string describe([NotNull] string list_description,
			[NotNull] string left_list_name,
			[NotNull] string right_list_name)
		{
			return string.Format("Difference in {0}. {1}{2}",
				list_description,
				_describe(left_list_name, in_left_but_not_right),
				_describe(right_list_name, in_right_but_not_left));
		}

		[NotNull]
		private static string _describe([NotNull] string list_name, [NotNull] List<T> list_unmatched_contents)
		{
			if (list_unmatched_contents.Count == 0) return string.Empty;
			return string.Format("{0}: [{1}].", list_name, string.Join(", ", list_unmatched_contents));
		}
	}
}
