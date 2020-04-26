using System;
using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayRunner
{
	public class IntervalReplayRunner : MonoBehaviour
	{


		[NonSerialized]
		public List<Translation> translations;

		private int _currentTranslationIndex;
		
		[NonSerialized]
		public List<CharacterAction> actions;

		private Character _character;


		[SerializeField]
		private FloatReference translationInterval;

		private Translation _nextTranslation;

		


		private float _timer;
		private AiPathfinder aiPathfinder;

		private float TimeLeftUntilNextInterval => translationInterval - _timer;

		private void Awake()
		{
			aiPathfinder = GetComponent<AiPathfinder>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref aiPathfinder, this, "Ai Pathfinder");
			
			_character = GetComponent<Character>();
			if (!Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _character, this, "Character")) enabled = false;
			
		}

		private void FixedUpdate()
		{
			_timer += Time.fixedDeltaTime;

			MoveTowardsNextTranslationData();
			

			if (_timer < translationInterval) return;

			_timer -= translationInterval;

			SetNextTranslationData();
			
			// Logging.Log("Moving towards (" + _nextTranslation.position.x + ", " + _nextTranslation.position.y + ")");

		}

		private void MoveTowardsNextTranslationData()
		{
			aiPathfinder.NextWaypoint = _nextTranslation;
			// MoveTowardsTranslationData(ref _nextTranslation);
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

	internal class AiPathfinder : MonoBehaviour
	{
		private CharacterMovement _characterMovement;
		
		private Translation nextWaypoint;
		private Transform _transform;

		public Vector3 currVelocity = Vector3.zero; // TODO: Shouldn't this be in CharacterMovement?

		public Translation NextWaypoint
		{
			get => nextWaypoint;
			set => nextWaypoint = value;
		}
		
		private void Awake()
		{
			_characterMovement = GetComponent<CharacterMovement>();
			_transform = GetComponent<Transform>();

		}
		private void Update()
		{
			MoveTowardsNextWaypoint();
		}
		
		
		private void MoveTowardsNextWaypoint()
		{
			// Vector3 newPos = Vector3.SmoothDamp(_transform.position, NextWaypoint.position, ref currVelocity, TimeLeftUntilNextInterval);
			// _transform.position = newPos;
			if (CheckIfCollisionBetweenCurrentPosAndWaypoint()) return;

			_characterMovement.AddForwardVelocity();

		}

		private bool CheckIfCollisionBetweenCurrentPosAndWaypoint()
		{
			return false;
		}
	}
}