namespace ClockBlockers.ReplaySystem.ReplayStorage
{
	// TODO: This should only be for *saving* replays.
	// IReplayRunner should be for recreating the replays (and therefore storing them). Probably even as a Queue
	public interface IReplayStorage {
		void ClearStorageForThisAct();
		// List<CharacterAction[]> GetAllRounds();
		CharacterAction[] GetNewestAct();
		CharacterAction[] GetCurrentAct();
		void SetActReplayData(CharacterAction[] actions);

		void SaveAction(Actions action, float[] parameters);
		void SaveAction(Actions action, float parameter);
		void SaveAction(Actions action);
		void StoreActData();
	}
}