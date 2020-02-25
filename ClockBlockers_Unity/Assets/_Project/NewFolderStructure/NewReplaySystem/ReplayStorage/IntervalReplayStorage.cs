using System.Collections.Generic;


namespace ClockBlockers.NewReplaySystem.ReplayStorage
{
	public class IntervalReplayStorage : IReplayStorage
	{
		private LinkedList<Translation> _translations;
		private LinkedList<CharacterAction> _characterActions;

		public IntervalReplayStorage(LinkedList<Translation> translations, LinkedList<CharacterAction> characterActions)
		{
			_characterActions = characterActions ?? new LinkedList<CharacterAction>();
			_translations = translations ?? new LinkedList<Translation>();
		}

		public void SaveTranslationData(Translation translation)
		{
			_translations.AddLast(translation);
		}

		public void SaveCharacterAction(CharacterAction characterAction)
		{
			_characterActions.AddLast(characterAction);
		}

		public void ClearStorageForThisAct()
		{
			throw new System.NotImplementedException();
		}

		public CharacterAction[] GetNewestAct()
		{
			throw new System.NotImplementedException();
		}

		public CharacterAction[] GetCurrentAct()
		{
			throw new System.NotImplementedException();
		}

		public void SetActActions(CharacterAction[] actions)
		{
			throw new System.NotImplementedException();
		}
	}
}