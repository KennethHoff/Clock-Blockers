namespace ClockBlockers.Characters
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
			replayRunner.StartRunning();
		}

		protected override void OnActEnded()
		{
			base.OnActEnded();
			Destroy(gameObject);
		}
	}
}