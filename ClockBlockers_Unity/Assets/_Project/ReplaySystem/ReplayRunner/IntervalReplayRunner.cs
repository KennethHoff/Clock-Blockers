using System;
using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayRunner
{
	[BurstCompile]
	public class IntervalReplayRunner : MonoBehaviour
	{
		
		// TODO: Look into the 'Command Pattern' << It looks quite good, and basically exactly what I was looking for.

		
		private List<Translation> _translations;

		private int _currentTranslationIndex = 0;

		
		private List<CharacterAction> _actions;

		private int _currentActionIndex = 0;

		private Character _character;


		[SerializeField]
		private FloatReference translationInterval = null;

		private float _timer;
		public int RemainingActions => _actions.Count - -_currentActionIndex;
		public int RemainingTranslations => _translations.Count - _currentTranslationIndex;

		private void Awake()
		{
			_character = GetComponent<Character>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _character, this, "Character");
		}

		public void Initialize(List<CharacterAction> replayStorageActions, List<Translation> replayStorageTranslations)
		{
			_actions = replayStorageActions;
			_translations = replayStorageTranslations;
		}

		public Translation? GetNextTranslationData()
		{ 
			_timer++;
			if (_timer < translationInterval) return null;

			_timer = 0;

			if (_translations == null || _currentTranslationIndex >= _translations.Count)
			{
				Unlink();
				return null;
			}
			
			Translation translation = _translations[_currentTranslationIndex];
			_currentTranslationIndex++;

			return _translations[_currentTranslationIndex];
			
		}

		/// <summary>
		/// The character unlinks if:
		/// There are no more actions and/or translations remaining (Both lists are 'completed'), or if the character fails to achieve an action or translation.
		/// </summary>
		private void Unlink()
		{
			Logging.Log("!!!!!!!!!!!!!  ReplayRunner UNLINKED  !!!!!!!!!");
		}
	}
}