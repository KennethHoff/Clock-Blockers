
using System.Collections;

using ClockBlockers.AI.AiControllers;

using UnityEngine;


namespace ClockBlockers.AI.States
{
	internal class Idle : AiState
	{
		public override IEnumerator Begin()
		{
			yield return new WaitForSeconds(1);
			
			aiController.SetState(new ReplicatingActions(aiController));
		}

		public Idle(AiController aiController) : base(aiController) { }
	}
}