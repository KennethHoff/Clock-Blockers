namespace ClockBlockers.ReplaySystem.ReplayStorage
{
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