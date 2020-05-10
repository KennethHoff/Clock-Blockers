using System.Collections;

using ClockBlockers.AI.AiControllers;
using ClockBlockers.AI.States;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayRunner
{
	
	
	/// <summary>
	/// After unlinking, this is the first 'AI controlled' state.
	/// The character should look like he just woke up - while being fully energized - and teleported somewhere he didn't recognize. 
	/// </summary>
	
	[BurstCompile]
	public class Unlinked : AiState
	{
		public override IEnumerator Begin()
		{
			
			// 'Recovering' animations
			Logging.Log(aiController.gameObject.name + " is fumbling around after regaining conscience");
			
			Logging.Log("Unlinked! Enabling AI in ...");
			
			Logging.Log("3...");
			yield return new WaitForSeconds(1);
			
			Logging.Log("2...");
			yield return new WaitForSeconds(1);
			
			Logging.Log("1...");
			yield return new WaitForSeconds(1);

			Logging.Log("AI enabled!");

			aiController.SetState(new FullyAutonomous(aiController));
		}

		public Unlinked(AiController aiController) : base(aiController) { }
	}
}