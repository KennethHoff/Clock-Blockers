namespace ClockBlockers.StateMachines
{
	// [CreateAssetMenu(fileName = "State Condition", menuName = "StateMachines/New Condition", order = 1)]
	public interface ICondition
	{
		bool Fulfilled();
	}
}