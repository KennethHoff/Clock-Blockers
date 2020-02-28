namespace ClockBlockers.ReplaySystem.ReplaySpawner {
	// TODO: Remove this. This is not the job of the character.
	public interface IReplaySpawner {
		// They all need to know what to spawn
		void SpawnLatestReplay();
		void SpawnAllReplays();
	}
}