namespace ClockBlockers.StateMachines
{
	// [CreateAssetMenu(fileName = "State", menuName = "StateMachines/New State", order = 0)]
	public interface IState
	{
		void Tick();
		
		void OnEnter();
		
		void OnExit();
	}
}