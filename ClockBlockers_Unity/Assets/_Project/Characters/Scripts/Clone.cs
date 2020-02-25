namespace ClockBlockers.Characters.Scripts
{
	public class Clone : Character
	{
		protected override void Start()
		{
			base.Start();
			EngageAllActions();
		}

		private void EngageAllActions()
		{
			replayRunner.Start();
		}

		protected override void OnActEnded()
		{
			base.OnActEnded();
			
			replayRunner.Stop();
		}
	}
}