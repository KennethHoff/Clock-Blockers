using ClockBlockers.ReplaySystem.ReplayRunner;


namespace ClockBlockers.StateMachines.Conditions
{
	internal class OutOfActions : ICondition
	{
		private readonly IntervalReplayRunner _replayRunner;
		public OutOfActions(IntervalReplayRunner replayRunner)
		{
			_replayRunner = replayRunner;
		}

		public bool Fulfilled()
		{
			return _replayRunner.RemainingActions == 0;
		}
	}
}