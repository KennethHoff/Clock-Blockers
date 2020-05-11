using ClockBlockers.ReplaySystem.ReplayRunner;


namespace ClockBlockers.StateMachines
{
	internal class OutOfThingsToDo : ICondition
	{
		private readonly IntervalReplayRunner _replayRunner;
		public OutOfThingsToDo(IntervalReplayRunner replayRunner)
		{
			_replayRunner = replayRunner;
		}

		public bool Fulfilled()
		{
			return _replayRunner.RemainingActions == 0 && _replayRunner.RemainingTranslations == 0;
		}
	}
}