using System.Collections;

using ClockBlockers.AI.AiControllers;
using ClockBlockers.Utility;


namespace ClockBlockers.AI.States
{
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