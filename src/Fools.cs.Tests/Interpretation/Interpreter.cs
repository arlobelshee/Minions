using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fools.cs.Tests.Interpretation
{
	public class Interpreter : IDisposable
	{
		private readonly FoolsLanguageApi _fools;
		private readonly List<Universe> _universes = new List<Universe>();
		private readonly List<Fool> _active_fools = new List<Fool>();
		private readonly TaskFactory _task_source;

		public Interpreter()
		{
			_fools = new FoolsLanguageApi(this);
			_task_source = new TaskFactory();
		}

		public FoolsLanguageApi fools { get { return _fools; } }

		public void Dispose()
		{
			stop_all_execution().Wait();
			_universes.ForEach(u => u.destroy());
		}

		private Task stop_all_execution()
		{
			return _task_source.ContinueWhenAll(_active_fools.Select(f => f.stop()).ToArray(), (results) => { });
		}

		internal void remember(Universe universe)
		{
			_universes.Add(universe);
		}

		internal void fool_always_requires(Fool who, Universe what_data, Universe.Access access_level)
		{
		}

		internal void begin_scheduling(Fool who)
		{
			_active_fools.Add(who);
		}

		internal Task create_task()
		{
			return _task_source.StartNew(() => { });
		}
	}
}