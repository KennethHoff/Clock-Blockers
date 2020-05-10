using System;
using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.Characters;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayStorage
{
	[BurstCompile]
	public class IntervalReplayStorage : MonoBehaviour
	{
		
		public List<Translation> translations;
		
		public List<CharacterAction> actions;


		private Character _character;
		private Transform _transform;

		[SerializeField]
		private FloatReference translationInterval = null;

		private float _timer;

		private void Awake()
		{
			_transform = GetComponent<Transform>();
			_character = GetComponent<Character>();
			
			actions = new List<CharacterAction>();
			translations = new List<Translation>();
		}

		private void FixedUpdate()
		{
			_timer += Time.fixedDeltaTime;

			if (_timer < translationInterval) return;
			
			_timer -= translationInterval;
			
			SaveTranslationData();
		}

		private void SaveTranslationData()
		{
			var translation = new Translation(_transform.position, _transform.rotation);
			translations.Add(translation);
		}
		
		public void SaveAction(Actions action, float[] parameters)
		{
			// throw new NotImplementedException();
		}

		public void SaveAction(Actions action, float parameter)
		{
			// throw new NotImplementedException();
		}

		public void SaveAction(Actions action)
		{
			// throw new NotImplementedException();
		}
	}
}