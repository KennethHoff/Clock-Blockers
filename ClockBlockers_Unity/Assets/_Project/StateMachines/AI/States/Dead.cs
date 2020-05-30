using ClockBlockers.Characters;


namespace ClockBlockers.StateMachines.AI.States
{
	internal class Dead : IState
	{
		private readonly Character _character;

		public Dead(Character character)
		{
			_character = character;
		}

		public void Tick() { }

		public void OnEnter()
		{
			_character.Kill();
		}

		public void OnExit() { }
	}
}