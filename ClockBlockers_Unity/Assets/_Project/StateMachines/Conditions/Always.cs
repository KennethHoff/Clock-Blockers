namespace ClockBlockers.StateMachines.Conditions
{
	public class Always : ICondition
	{
		public bool Fulfilled()
		{
			return true;
		}
	}
}