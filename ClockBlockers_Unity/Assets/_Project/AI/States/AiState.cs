using System.Collections;

using ClockBlockers.AI.AiControllers;

namespace ClockBlockers.AI.States
{
	public abstract class AiState
	{
		protected AiController aiController;
		protected AiState(AiController aiController)
		{
			this.aiController = aiController;
		}

		public virtual IEnumerator Begin()
		{
			yield break;
		}

		public virtual void End() { }
		public virtual void CompletedJob() { }
	}
}