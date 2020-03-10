using System;
using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.ToBeMoved;
using ClockBlockers.Utility;

using UnityEngine;
using UnityEngine.AI;


namespace ClockBlockers.ReplaySystem.ReplayRunner
{
	public class IntervalReplayRunner : MonoBehaviour
	{

		private CharacterMovement _characterMovement;

		private NavMeshAgent _navMeshAgent;
		
		[NonSerialized]
		public List<Translation> translations;

		private int _currentTranslationIndex;
		
		[NonSerialized]
		public List<CharacterAction> actions;
		
		
		private Character _character;
		private Transform _transform;

		[SerializeField]
		private CameraController cameraController;

		[SerializeField]
		private FloatReference translationInterval;

		private Translation _nextTranslation;
		private Translation _prevTranslation;


		private float _timer;

		private float _timeLeftUntilNextInterval;

		private void Awake()
		{
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_character = GetComponent<Character>();
			_transform = GetComponent<Transform>();
			_characterMovement = GetComponent<CharacterMovement>();
		}

		private void FixedUpdate()
		{
			_timer += Time.fixedDeltaTime;

			// _timeLeftUntilNextInterval = translationInterval - _timer;
			


			if (_timer < translationInterval) return;
			
			_timer -= translationInterval;

			SetNextTranslationData();
		}

		private void SetNextTranslationData()
		{
			
			// if (Mathf.Abs(_transform.position.magnitude - _nextTranslation.position.magnitude) > 0.25f)
			// {
			// 	Unlink();
			// }
			_prevTranslation = _nextTranslation;

			_nextTranslation = translations[_currentTranslationIndex];

			_navMeshAgent.SetDestination(_nextTranslation.position);
			
			
			_currentTranslationIndex++;

			if (_currentTranslationIndex >= translations.Count)
			{
				Unlink();
			}

		}

		public void Begin()
		{
			
		}

		public void End()
		{
			
		}

		public void Unlink()
		{
			Logging.Log(name + " unlinked!", this);
			enabled = false;
		}
	}
}