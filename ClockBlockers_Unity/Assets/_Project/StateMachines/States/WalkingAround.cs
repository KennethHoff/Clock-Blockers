using ClockBlockers.AI;


namespace ClockBlockers.StateMachines.States
{
	internal class WalkingAround : IState
	{
		private readonly AiPathfinder _aiPathfinder;
		
		private const float RandomPositionDistance = 10f; 
		public WalkingAround(AiPathfinder aiPathfinder)
		{
			_aiPathfinder = aiPathfinder;
		}

		public void Tick()
		{
			_aiPathfinder.Tick();

			if (_aiPathfinder.OnAPath) return;

			_aiPathfinder.RequestPathToRandomLocationWithinDistance(RandomPositionDistance);
			
		}
		public void OnEnter() { }
		public void OnExit() { }
	}
}