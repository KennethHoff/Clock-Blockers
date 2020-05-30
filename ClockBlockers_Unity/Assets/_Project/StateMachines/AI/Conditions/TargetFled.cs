namespace ClockBlockers.StateMachines.AI.Conditions
{
	internal class TargetFled : ICondition
	{
		// TODO: Rework this. Currently all it does is check if the enemy is *not* spotted.
		private readonly ICondition _enemySpotted;

		public TargetFled(ICondition enemySpotted)
		{
			_enemySpotted = enemySpotted;
		}
		public bool Fulfilled()
		{
			return !_enemySpotted.Fulfilled();
		}
	}
}