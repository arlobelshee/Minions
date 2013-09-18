// NanoPass.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Fools.cs.Utilities;

namespace Fools.cs.TransformAst
{
	public abstract class NanoPass<TTarget>
	{
		[NotNull] private static readonly ReadOnlyCollection<NanoPass<TTarget>> _all;
		[NotNull] private readonly ReadOnlyCollection<AstStateCondition> _requires;

		static NanoPass()
		{
			_all = Assembly.GetExecutingAssembly()
				.GetTypes()
				// ReSharper disable PossibleNullReferenceException
				.Where(t => t.IsSubclassOf(typeof (NanoPass<TTarget>)) && !t.IsAbstract)
				// ReSharper restore PossibleNullReferenceException
				.Select(t => {
					// ReSharper disable PossibleNullReferenceException
					var constructor = t.GetConstructor(new Type[] {});
					// ReSharper restore PossibleNullReferenceException
					Debug.Assert(constructor != null,
						"Programmmer error: NanoPasses are required to have default constructors.",
						"{0} is missing a default constructor. Please add one.",
						t.FullName);
					return (NanoPass<TTarget>) constructor.Invoke(new object[] {});
				})
				.ToList()
				.AsReadOnly();
		}

		protected NanoPass([NotNull] ReadOnlyCollection<AstStateCondition> requires)
		{
			_requires = requires;
		}

		[NotNull]
		public static ReadOnlyCollection<NanoPass<TTarget>> all { get { return _all; } }

		[NotNull]
		// ReSharper disable ReturnTypeCanBeEnumerable.Global
		public ReadOnlyCollection<AstStateCondition> requires
		{
			get { return _requires; }
		}

		// ReSharper restore ReturnTypeCanBeEnumerable.Global

		[NotNull]
		public abstract TTarget run([NotNull] TTarget data, [NotNull] Action<AstStateCondition> add_condition);
	}
}
