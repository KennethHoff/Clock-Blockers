using ClockBlockers.AI.AiControllers;


namespace ClockBlockers.StateMachines.AI.Conditions
{
	// This doesn't feel right - The first time I've wanted to store references to other fields in a field.
	internal class ControlRegained : ICondition
	{
		private readonly BaseAiController _baseAiController;

		public ControlRegained(BaseAiController baseAiController)
		{
			_baseAiController = baseAiController;
		}

		public bool Fulfilled()
		{
			return _baseAiController.controllingPlayer == null;
		}
	}
}