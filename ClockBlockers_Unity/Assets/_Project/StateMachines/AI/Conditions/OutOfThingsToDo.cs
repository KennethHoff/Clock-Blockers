using ClockBlockers.AI;


namespace ClockBlockers.StateMachines.AI.Conditions
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