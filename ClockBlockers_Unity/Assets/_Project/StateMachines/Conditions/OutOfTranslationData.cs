using ClockBlockers.ReplaySystem.ReplayRunner;


namespace ClockBlockers.StateMachines.Conditions
{
	internal class OutOfTranslationData : ICondition
	{
		private readonly IntervalReplayRunner _replayRunner;
		public OutOfTranslationData(IntervalReplayRunner replayRunner)
		{
			_replayRunner = replayRunner;
		}

		public bool Fulfilled()
		{
			return _replayRunner.RemainingTranslations == 0;
		}
	}
}