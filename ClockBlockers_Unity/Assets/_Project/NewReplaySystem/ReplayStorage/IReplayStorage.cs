namespace ClockBlockers.NewReplaySystem.ReplayStorage
{
	public interface IReplayStorage {
		void ClearStorageForThisAct();
		// List<CharacterAction[]> GetAllRounds();
		CharacterAction[] GetNewestAct();
		CharacterAction[] GetCurrentAct();
		void SetActActions(CharacterAction[] actions);
	}
}