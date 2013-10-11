// FoolFactory.cs
// 
// Copyright 2012 The Minions Project (http:/github.com/Minions).
// All rights reserved. Usage as permitted by the LICENSE.txt file for this project.

using System;
using System.Threading;
using System.Threading.Tasks;
using Fools.cs.Utilities;

namespace Fools.cs.Api
{
	public class FoolFactory : IDisposable
	{
		[NotNull] private readonly TaskFactory _task_factory;
		[NotNull] private readonly CancellationTokenSource _cancellation;

		private FoolFactory(TaskScheduler scheduler)
		{
			_cancellation = new CancellationTokenSource();
			_task_factory = new TaskFactory(_cancellation.Token,
				TaskCreationOptions.PreferFairness,
				TaskContinuationOptions.None,
				scheduler);
		}

		[NotNull]
		public static FoolFactory using_background_threads()
		{
			return new FoolFactory(TaskScheduler.Default);
		}

		/// <summary>
		/// Intended to be called from the UI thread. Will ensure that all
		/// created fools run on the same thread as the one that calls this
		/// method.
		/// 
		/// Will fail when run from some kinds of threads (unspecified exactly
		/// which ones), but will always work from the UI thread.
		/// </summary>
		[NotNull]
		public static FoolFactory on_this_thread()
		{
			return new FoolFactory(TaskScheduler.FromCurrentSynchronizationContext());
		}

		[NotNull]
		public Fool<TLab> create_fool<TLab>([NotNull] TLab lab) where TLab : class
		{
			return new Fool<TLab>(_task_factory.StartNew(noop), lab);
		}

		public static void noop() {}

		public void Dispose()
		{
			_cancellation.Cancel();
			_cancellation.Dispose();
		}
	}
}
