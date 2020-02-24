namespace ClockBlockers.Characters
{
	public class CloneController : BaseController
	{
		protected override void Start()
		{
			base.Start();
			EngageAllActions();
		}

		private void EngageAllActions()
		{
			actionRunner.EngageAllActions(actionStorage.ReplayActions);
		}
	}
}