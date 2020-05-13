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

		private List<CharacterAction> _actions;

		private Character _character;


		[SerializeField]
		public FloatReference translationInterval = null;

		private float _timer;
		public bool Unlinked { get; set; } = false;


		private void Awake()
		{
			_character = GetComponent<Character>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(_character, this, "Character");
		}

		public void Initialize(List<CharacterAction> replayStorageActions, List<Translation> replayStorageTranslations)
		{
			_actions = replayStorageActions;
			_translations = replayStorageTranslations;
		}

		public List<Vector3> CreatePositionListFromTranslations()
		{
			List<Vector3> allPositions = _translations.Select(translation => translation.position).ToList();
			return allPositions;
		}
	}
}