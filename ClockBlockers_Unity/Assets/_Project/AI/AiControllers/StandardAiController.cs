using ClockBlockers.AI.States;


namespace ClockBlockers.AI.AiControllers
{
	public class StandardAiController : AiController
	{
		public override void Begin()
		{
			SetState(new Idle(this));
		}
		public override void End() { }
	}
}