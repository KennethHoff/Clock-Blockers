using System;
using System.Collections.Generic;
using System.Linq;

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
		public bool Unlinked { get; set; } = false;

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

		public Translation? GetTranslationData()
		{
			_timer += Time.deltaTime;

			if (_timer < translationInterval) return null;
			_timer -= translationInterval;
			
			Translation resultTranslation = _translations[_currentTranslationIndex++];
			
			return resultTranslation;
		}

		public List<Translation> GetAllTranslations()
		{
			return _translations;
		}

		public List<Vector3> CreatePositionListFromTranslations()
		{
			List<Vector3> allPositions = _translations.Select(translation => translation.position).ToList();
			return allPositions;
		}
	}
}