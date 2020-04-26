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

		public Vector3 currVelocity = Vector3.zero;
		


		private float _timer;

		private float TimeLeftUntilNextInterval => translationInterval - _timer;

		private void Awake()
		{
			_character = GetComponent<Character>();
			_transform = GetComponent<Transform>();
			_characterMovement = GetComponent<CharacterMovement>();
		}

		private void FixedUpdate()
		{
			_timer += Time.fixedDeltaTime;

			MoveTowardsNextTranslationData();
			

			if (_timer < translationInterval) return;

			_timer -= translationInterval;

			SetNextTranslationData();
			
			Logging.Log("Moving towards (" + _nextTranslation.position.x + ", " + _nextTranslation.position.y + ")");

		}

		private void MoveTowardsNextTranslationData()
		{
			MoveTowardsTranslationData(ref _nextTranslation);
		}

		private void MoveTowardsTranslationData(ref Translation tData)
		{
			Vector3 newPos = Vector3.SmoothDamp(_transform.position, tData.position, ref currVelocity, TimeLeftUntilNextInterval);
			_transform.position = newPos;
		}

		/// <summary>
		/// Sets the target translation; Where the clone wants to go towards.
		/// </summary>
		private void SetNextTranslationData()
		{
			
			// if (Mathf.Abs(_transform.position.magnitude - _nextTranslation.position.magnitude) > 0.25f)
			// {
			// 	Unlink();
			// }

			if (translations.Count <= 0)
			{
				Unlink();
				return;
			}
			
			_nextTranslation = translations[_currentTranslationIndex];

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
			
			_character.Kill();

		}
	}
}