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
	public class ReplayRunner : MonoBehaviour
	{
		
		[NonSerialized]
		public List<Translation> translations;

		private int _currentTranslationIndex;
		
		[NonSerialized]
		public List<CharacterAction> actions;

		private Character _character;

		[SerializeField]
		private FloatReference translationInterval = null;

		private float _timer;
		
		private void Awake()
		{
			_character = GetComponent<Character>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _character, this, "Character");
		}

		public Translation? GetNextTranslationData()
		{ 
			_timer++;
			if (_timer < translationInterval) return null;

			_timer = 0;

			if (translations == null || _currentTranslationIndex >= translations.Count)
			{
				Unlink();
				return null;
			}
			
			Translation translation = translations[_currentTranslationIndex];
			_currentTranslationIndex++;

			return translations[_currentTranslationIndex];
			
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