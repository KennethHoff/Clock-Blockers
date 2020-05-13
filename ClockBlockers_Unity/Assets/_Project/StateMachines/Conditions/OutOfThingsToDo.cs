using ClockBlockers.AI;
using ClockBlockers.ReplaySystem.ReplayRunner;


namespace ClockBlockers.StateMachines.Conditions
{
	internal class OutOfThingsToDo : ICondition
	{
		private readonly AiPathfinder _aiPathfinder;

		public OutOfThingsToDo(AiPathfinder aiPathfinder)
		{
			_aiPathfinder = aiPathfinder;
		}

		public bool Fulfilled()
		{
			return _aiPathfinder.hasCompletedAPath && _aiPathfinder.NotOnAPath;
		}
	}
}