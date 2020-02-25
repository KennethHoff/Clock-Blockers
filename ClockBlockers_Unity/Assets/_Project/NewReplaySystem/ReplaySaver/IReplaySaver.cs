namespace ClockBlockers.NewReplaySystem.ReplaySaver
{
	public interface IReplaySaver
	{
		void SaveAction(CharacterAction characterAction);
		void PushActDataToRound();
	}
}