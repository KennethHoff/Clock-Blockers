using ClockBlockers.AI;
using ClockBlockers.Utility;


namespace ClockBlockers.StateMachines.States
{
	internal class ExternalControl : IState
	{
		private readonly AiPathfinder _aiPathfinder;
		public ExternalControl(AiPathfinder aiPathfinder)
		{
			_aiPathfinder = aiPathfinder;
		}

		public void Tick()
		{
			_aiPathfinder.Tick();
		}

		public void OnEnter()
		{
			Logging.Log("AI is now under external control");
		}

		public void OnExit()
		{
			Logging.LogWarning("AI is no longer under external control");
		}
	}
}