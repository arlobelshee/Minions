// Fool.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class Fool<TLab> where TLab : class
	{
		[NotNull] private Task _previous_operation;

		[NotNull] private readonly TLab _lab;
		[NotNull] private readonly object _task_lock = new object();
		[NotNull] private readonly List<Action<TLab>> _upon_completion = new List<Action<TLab>>();

		public Fool([NotNull] Task root_task, [NotNull] TLab lab)
		{
			_previous_operation = root_task;
			_lab = lab;
		}

		public void do_work([NotNull] Action<TLab> work, [NotNull] Action done)
		{
			_do_work(work, done);
		}

		/// <summary>
		///    Must be called by a thread that is currently executing an action for this Fool. No
		///    attempt is made to ensure that this requirement is met.
		/// </summary>
		/// <param name="work"></param>
		public void upon_completion_of_this_task([NotNull] Action<TLab> work)
		{
			_upon_completion.Add(work);
		}

		private void _do_work([NotNull] Action<TLab> work, [NotNull] Action done)
		{
			Action<Task> next_operation = ignored_result_of_previous_task =>
			{
				try
				{
					work(_lab);
					_upon_completion.ForEach(w => // ReSharper disable PossibleNullReferenceException
						w(_lab));
					// ReSharper restore PossibleNullReferenceException
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					// TODO: What about exceptions thrown by the action? When should they be observed?
				}
				_upon_completion.Clear();
				done();
			};

			lock (_task_lock)
			{
				_previous_operation = _previous_operation.ContinueWith(next_operation);
			}
		}
	}
}
