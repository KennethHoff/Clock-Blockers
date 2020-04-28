using System.Collections;

using ClockBlockers.AI.AiControllers;
using ClockBlockers.ReplaySystem.ReplayRunner;


namespace ClockBlockers.AI.States
{
	public class ReplicatingActions : AiState
	{
		public override IEnumerator Begin()
		{
			aiController.replayRunner.enabled = true;
			aiController.replayRunner.enabledState = this;
			aiController.aiPathfinder.EnablePathfinding();
			yield break;
		}

		public override void End()
		{
			aiController.replayRunner.enabled = false;
			aiController.aiPathfinder.ChangeDestination(default);
			aiController.aiPathfinder.DisablePathFinding();

		}

		public override void CompletedJob()
		{
			aiController.SetState(new Unlinked(aiController));
		}

		public ReplicatingActions(AiController aiController) : base(aiController) { }
	}
}