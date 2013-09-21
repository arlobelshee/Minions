using Fools.cs.Api;
using Fools.cs.Utilities;

namespace core_compile
{
	public class CompileOneProject
	{
		[NotNull] private readonly MissionLocation _mission_control;

		private CompileOneProject([NotNull] MissionLocation mission_control)
		{
			_mission_control = mission_control;
		}

		public static void submit_missions_to([NotNull] MissionLocation mission_control)
		{
			var compile_a_project_when_found =
				new MissionDescription<CompileOneProject>(() => new CompileOneProject(mission_control));

			mission_control.send_out_fools_to(compile_a_project_when_found);
		}
	}
}