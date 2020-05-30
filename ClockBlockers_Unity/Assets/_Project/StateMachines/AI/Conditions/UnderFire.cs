using ClockBlockers.ToBeMoved;


namespace ClockBlockers.StateMachines.AI.Conditions
{
	internal class UnderFire : ICondition
	{
		// LOW-PRIO: This needs to use some better logic than simply checking if 'was hit' - Ideally it would be true if a shot hit near / Was shot past
		private float _healthLastFrame;

		private readonly HealthComponent _healthComponent;

		public UnderFire(HealthComponent healthComponent)
		{
			_healthComponent = healthComponent;
		}

		public bool Fulfilled()
		{
			if (_healthComponent.Health < _healthLastFrame)
			{
				_healthLastFrame = _healthComponent.Health;
				return true;
			}

			_healthLastFrame = _healthComponent.Health;
			return false;
		}
	}
}