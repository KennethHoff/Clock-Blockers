namespace ClockBlockers.NewReplaySystem.ReplaySpawner
{
	public interface IReplaySpawner {
		// They all need to know what to spawn
		void SpawnLatestReplay();
		void SpawnAllReplays();
	}
	
}