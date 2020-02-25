namespace ClockBlockers.NewReplaySystem.ReplayRunner
{
	internal interface IReplayRunner
	{
		// Turn off Coroutines etc ...
		void Stop();
		void Start();
	}
}