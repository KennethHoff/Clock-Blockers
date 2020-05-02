using System;
using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.AI.AiControllers;
using ClockBlockers.AI.States;
using ClockBlockers.Characters;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayRunner
{
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
		
		private AiController aiController;

		public AiState enabledState;

		// private float TimeLeftUntilNextInterval => translationInterval - _timer;

		private void Awake()
		{
			aiController = GetComponent<AiController>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref aiController, this, "Ai Controller");
			
			// aiPathfinder = GetComponent<AiPathfinder>();
			// Logging.CheckIfCorrectMonoBehaviourInstantiation(ref aiPathfinder, this, "Ai Pathfinder");
			
			_character = GetComponent<Character>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _character, this, "Character");

		}

		private void FixedUpdate()
		{
			_timer += Time.fixedDeltaTime;

			if (_timer < translationInterval) return;

			_timer -= translationInterval;
			
			Translation nextTranslation = GetNextTranslationData();

			aiController.aiPathfinder.ChangeDestination(nextTranslation);

			Logging.Log("Moving towards (" + nextTranslation.position.x + ", " + nextTranslation.position.y + ")");

		}

		/// <summary>
		/// Sets the target translation; Where the clone wants to go towards.
		/// </summary>
		private Translation GetNextTranslationData()
		{
			// ReSharper disable once InvertIf
			if ( (translations.Count == 0) || (_currentTranslationIndex+1 > translations.Count) )
			{
				Unlink();
				return default;
			}
			
			return translations[_currentTranslationIndex++];
		}

		
		/// <summary>
		/// The character unlinks if:
		/// There are no more actions and/or translations remaining (Both lists are 'completed')
		/// The character fails to achieve an action or translation.
		/// </summary>
		private void Unlink()
		{
			enabledState.CompletedJob();
		}
	}
}