using ClockBlockers.AI.AiControllers;


namespace ClockBlockers.StateMachines
{
	// This doesn't feel right - The first time I've wanted to store references to other fields in a field.
	internal class ControlLost : ICondition
	{
		private readonly AiController _aiController;
		public ControlLost(AiController aiController)
		{
			this._aiController = aiController;
		}

		public bool Fulfilled()
		{
			return _aiController.controllingPlayer != null;
		}
	}
}