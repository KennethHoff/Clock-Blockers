using System;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.GameControllers;
using ClockBlockers.MatchData;
using ClockBlockers.ReplaySystem;
using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.ToBeMoved;
using ClockBlockers.Utility;
using ClockBlockers.Weapons;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace ClockBlockers.Input
{
	[RequireComponent(typeof(Character))]
	[RequireComponent(typeof(CharacterMovement))]
	internal class PlayerInputController : MonoBehaviour
	{
		private CharacterMovement _characterMovement;

		private Character _character;

		private IntervalReplayStorage _replayStorage;

		[SerializeField]
		private CameraController cameraController;

		[SerializeField]
		private Gun gun;

		[SerializeField]
		private FloatReference verticalMouseSensitivity;

		[SerializeField]
		private FloatReference horizontalMouseSensitivity;

		private Vector2 _moveInput;
		private float _sideToSideCharacterRotation;
		private float _upDownCameraRotation;
		private bool _inputEnabled;

		
		private void Awake()
		{
			_replayStorage = GetComponent<IntervalReplayStorage>();
			_characterMovement = GetComponent<CharacterMovement>();
			_character = GetComponent<Character>();

			_character.onKilled += CharacterDied;
		}

		private void CharacterDied()
		{
			SetCursorActive(true);
		}

		private void Update()
		{

			_characterMovement.Rotate(_sideToSideCharacterRotation);
			cameraController.Rotate(_upDownCameraRotation);
		}

		private void FixedUpdate()
		{

			_characterMovement.MoveForward(_moveInput);
		}


		private void OnEnable()
		{
			SetCursorActive(false);
		}

		[UsedImplicitly]
		private void OnLook(InputValue ctx)
		{
			if (!_inputEnabled)
			{
				_sideToSideCharacterRotation = 0;
				_upDownCameraRotation = 0;
				return;
			}
			var value = ctx.Get<Vector2>();

			_sideToSideCharacterRotation = value.x * horizontalMouseSensitivity * Time.deltaTime;
			_upDownCameraRotation = value.y * verticalMouseSensitivity * Time.deltaTime;
			
		}


		[UsedImplicitly]
		private void OnMovement(InputValue ctx)
		{
			if (!_inputEnabled)
			{
				_moveInput = Vector2.zero;
				return;
			}
			_moveInput = ctx.Get<Vector2>();
		}

		[UsedImplicitly]
		private void OnShoot()
		{
			if (!_inputEnabled) return;
			gun.PullTrigger();

			_replayStorage.SaveAction(Actions.Shoot);
		}

		[UsedImplicitly]
		private void OnStartNewAct() // Obviously a test feature
		{
			if (!_inputEnabled) return;
			FindObjectOfType<Act>().Remove();
		}

		[UsedImplicitly]
		private void OnSpawn() // Obviously a test feature
		{
			FindObjectOfType<Act>().SpawnNewClone(); // Really messy.
		}

		[UsedImplicitly]
		private void OnIncreaseTimescale() // Obviously a test feature
		{
			Time.timeScale += 1;
			Logging.Log("Increasing timescale. Now at: " + Time.timeScale);
		}

		[UsedImplicitly]
		private void OnDecreaseTimescale() // Obviously a test feature
		{
			Time.timeScale -= 1;
			Logging.Log("Decreasing timescale. Now at: " + Time.timeScale);
		}

		[UsedImplicitly]
		private void OnToggleCursor()
		{
			ToggleCursor();
		}

		private void ToggleCursor()
		{
			SetCursorActive(!_inputEnabled);
		}

		private void SetCursorActive(bool toggle)
		{
			LockCursor(!toggle);
			_inputEnabled = !toggle;
		}


		private static void LockCursor(bool locked)
		{
			Cursor.lockState = locked
				? CursorLockMode.Locked
				: CursorLockMode.None;
		}
	}
}