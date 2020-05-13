using ClockBlockers.ReplaySystem.ReplayRunner;


namespace ClockBlockers.StateMachines.Conditions
{
	public class HasThingsToDo : ICondition
	{
		private readonly IntervalReplayRunner _replayRunner;
		public HasThingsToDo(IntervalReplayRunner replayRunner)
		{
			_replayRunner = replayRunner;
		}

		public bool Fulfilled()
		{
			return _replayRunner.RemainingActions > 0 || _replayRunner.RemainingTranslations > 0;
		}
	}
}