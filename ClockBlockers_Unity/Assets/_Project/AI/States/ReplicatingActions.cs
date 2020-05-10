using System.Collections;

using ClockBlockers.AI.AiControllers;
using ClockBlockers.ReplaySystem;
using ClockBlockers.ReplaySystem.ReplayRunner;

using Unity.Burst;

namespace ClockBlockers.AI.States
{
	[BurstCompile]
	public class ReplicatingActions : AiState
	{
		public ReplicatingActions(AiController aiController) : base(aiController) { }

		public override IEnumerator Begin()
		{
			aiController.replayRunner.enabled = true;
			yield break;
		}

		public override void Update()
		{
			Translation? newDestination = aiController.replayRunner.GetNextTranslationData();
			if (newDestination.HasValue)
			{
				aiController.aiPathfinder.RequestPath(newDestination.Value.position);
			}
			
			aiController.aiPathfinder.RunPathfinding();
		}

		public override void End()
		{
			aiController.replayRunner.enabled = false;
		}

		public override void CompletedJob()
		{
			aiController.SetState(new Unlinked(aiController));
		}
	}
}