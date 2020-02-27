namespace ClockBlockers.ReplaySystem.ReplayRunner
{
	internal interface IReplayRunner
	{
		// Turn off Coroutines etc ...
		void Begin();
		void End();
	}
}