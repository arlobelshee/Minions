using System;

namespace Fools.cs.Tests.Interpretation
{
	public class Maybe<T>
	{
		private T _value;
		private bool _has_value;

		private Maybe(T value, bool has_value)
		{
			_value = value;
			_has_value = has_value;
		}

		public T value
		{
			get
			{
				Require.is_true(
					_has_value,
					() => new InvalidOperationException("Attempted to directly access the value of a Maybe that has no value."));
				return _value;
			}
		}

		public static Maybe<T> just(T value)
		{
			Require.not_null(value, "Maybe's value");
			return new Maybe<T>(value, true);
		}

		public void assign(T new_value)
		{
			Require.not_null(new_value, "Maybe's value");
			_value = new_value;
			_has_value = true;
		}

		public static Maybe<T> not()
		{
			return new Maybe<T>(default(T), false);
		}
	}
}