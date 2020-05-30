using ClockBlockers.ToBeMoved;


namespace ClockBlockers.StateMachines.AI.Conditions
{
	internal class Died : ICondition
	{
		private readonly HealthComponent _healthComponent;

		public Died(HealthComponent healthComponent)
		{
			_healthComponent = healthComponent;
		}

		public bool Fulfilled()
		{
			return _healthComponent.Dead;
		}
	}
}