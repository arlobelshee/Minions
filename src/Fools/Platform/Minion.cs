namespace Fools.Platform
{
	public abstract class Minion
	{
		protected readonly MinionBoss task_master;
		protected Universe my_universe;
		private readonly UniverseAccessSet _universes;

		protected Minion(MinionBoss task_master)
		{
			this.task_master = task_master;
			my_universe = new Universe();
			_universes = new UniverseAccessSet();
			_universes.include_write_access(my_universe);
		}

		public UniverseAccessSet universe_awareness { get { return _universes; } }

		public abstract WorkSchedule do_thy_masters_bidding(Message message);

		protected void resume_handling_messages()
		{
			task_master.i_await_your_bidding(this);
		}
	}

	public class Universe
	{
	}

	public class UniverseAccessSet
	{
		public void include_write_access(Universe universe)
		{
		}
	}
}
