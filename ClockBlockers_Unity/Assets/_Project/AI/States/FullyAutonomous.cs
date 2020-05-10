using System.Collections;

using ClockBlockers.AI.AiControllers;
using ClockBlockers.Utility;

using Unity.Burst;


namespace ClockBlockers.AI.States
{
	[BurstCompile]
	internal class FullyAutonomous : AiState
	{
		public override IEnumerator Begin()
		{
			Logging.Log("AI is now fully autonomous");
			yield break;
		}

		public FullyAutonomous(AiController aiController) : base(aiController) { }
	}
}