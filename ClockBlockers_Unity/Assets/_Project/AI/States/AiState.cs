using System.Collections;

using ClockBlockers.AI.AiControllers;

using Unity.Burst;


namespace ClockBlockers.AI.States
{
	[BurstCompile]
	public abstract class AiState
	{
		protected readonly AiController aiController;
		protected AiState(AiController aiController)
		{
			this.aiController = aiController;
		}

		public virtual IEnumerator Begin()
		{
			yield break;
		}

		public virtual void Update() { }

		public virtual void End() { }
		public virtual void CompletedJob() { }
	}
}