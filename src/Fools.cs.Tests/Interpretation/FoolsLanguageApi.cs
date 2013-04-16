using System;

namespace Fools.cs.Tests.Interpretation
{
	public class FoolsLanguageApi
	{
		private readonly Interpreter _interpreter;

		internal FoolsLanguageApi(Interpreter interpreter)
		{
			_interpreter = interpreter;
		}

		public Building create_building()
		{
			return new Building(_interpreter);
		}

		public Mission define_mission(
			string name,
			BuildingAccess[] whole_mission_locations,
			Mission.CompletionCriterion mission_is_done,
			ResourceBudget whole_mission_budget,
			Cleanup cleanup)
		{
			throw new NotImplementedException();
		}
	}
}
