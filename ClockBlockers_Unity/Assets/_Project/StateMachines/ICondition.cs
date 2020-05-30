namespace ClockBlockers.StateMachines
{
	// [CreateAssetMenu(fileName = "State Condition", menuName = "StateMachines/New Condition", order = 1)]
	
	// TODO: Find a way to 'negate' an condition 'fulfilled'.
	// The following is how conditions are used:
	// AddTransition(<from state>, <to state>, <Condition>);
	// This would change the State Machine from <From state> to <To state> as soon as <Condition> is fulfilled.
	
	// Ideally, I could simply do the following, and have that work as you'd expect (Looking at it from a natural-language perspective)
	// AddTransition(<from state>, <to state>, !<Condition>);
	
	// I tried turning the ICondition interface into an abstract class, and overloading the ! operator (Having it return !Fulfilled), but that didn't really work as I would like.
	
	// The most reasonable implementation would be to add a new 'NotFulfilled' method, and call that instead whenever it's set to false somehow.


	public interface ICondition
	{
		bool Fulfilled();
	}
}