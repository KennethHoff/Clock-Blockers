using System;

using Between_Names.Property_References;

using ClockBlockers.AI;
using ClockBlockers.Characters;
using ClockBlockers.Events;
using ClockBlockers.ReplaySystem;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.ToBeMoved;
using ClockBlockers.Utility;
using ClockBlockers.Weapons;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.InputSystem;

namespace ClockBlockers.Input
{
	[RequireComponent(typeof(Character))]
	[RequireComponent(typeof(CharacterMovement))]
	public class PlayerInputController : MonoBehaviour
	{
		private CharacterMovement _characterMovement;

		private Character _character;

		private IntervalReplayStorage _replayStorage;

		[SerializeField]
		private CameraController cameraController;

		[SerializeField]
		private Gun gun;

		private StandardAiPathfinder testing;

		[SerializeField]
		private FloatReference verticalMouseSensitivity;

		[SerializeField]
		private FloatReference horizontalMouseSensitivity;

		private Vector2 _moveInput;
		private float _sideToSideCharacterRotation;
		private float _upDownCameraRotation;
		private bool _inputEnabled;

		[SerializeField]
		private GameEvent endActEvent;
		
		private void Awake()
		{
			_replayStorage = GetComponent<IntervalReplayStorage>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _replayStorage, this, "Replay Storage");

			_characterMovement = GetComponent<CharacterMovement>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _characterMovement, this, "Character Movement");

			_character = GetComponent<Character>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _character, this, "Character");

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

			if (_moveInput.magnitude > 0)
			{
				_characterMovement.AddVelocityRelativeToForward(_moveInput);
			}
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
		private void OnJump()
		{
			_characterMovement.Jump();
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
			endActEvent.Raise();
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