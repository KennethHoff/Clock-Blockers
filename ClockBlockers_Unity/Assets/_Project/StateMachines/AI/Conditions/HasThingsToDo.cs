using ClockBlockers.ReplaySystem.ReplayRunner;


namespace ClockBlockers.StateMachines.AI.Conditions
{
	internal class HasThingsToDo : ICondition
	{
		private readonly IntervalReplayRunner _replayRunner;

		public HasThingsToDo(IntervalReplayRunner replayRunner)
		{
			_replayRunner = replayRunner;
		}

		public bool Fulfilled()
		{
			return _replayRunner.Unlinked == false;
		}
	}
}