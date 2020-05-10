using ClockBlockers.AI.States;

using Unity.Burst;


namespace ClockBlockers.AI.AiControllers
{
	[BurstCompile]
	public class StandardAiController : AiController
	{
		public override void Begin()
		{
			SetState(new Idle(this));
		}
		public override void End() { }
	}
}