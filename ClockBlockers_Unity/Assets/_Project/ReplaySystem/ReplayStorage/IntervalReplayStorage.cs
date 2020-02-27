using System;
using System.Collections.Generic;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayStorage
{
	public class IntervalReplayStorage : MonoBehaviour, IReplayStorage
	{
		private readonly LinkedList<Translation> _translations;
		private readonly LinkedList<CharacterAction> _characterActions;

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
			throw new NotImplementedException();
		}

		public CharacterAction[] GetNewestAct()
		{
			throw new NotImplementedException();
		}

		public CharacterAction[] GetCurrentAct()
		{
			throw new NotImplementedException();
		}

		public void SetActReplayData(CharacterAction[] actions)
		{
			throw new NotImplementedException();
		}

		public void SaveAction(Actions action, float[] parameters)
		{
			throw new NotImplementedException();
		}

		public void SaveAction(Actions action, float parameter)
		{
			throw new NotImplementedException();
		}

		public void SaveAction(Actions action)
		{
			throw new NotImplementedException();
		}

		public void StoreActData()
		{
			throw new NotImplementedException();
		}
	}
}