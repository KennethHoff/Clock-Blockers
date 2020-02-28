using System;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.GameControllers;
using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.ToBeMoved;
using ClockBlockers.Weapons;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace ClockBlockers.Input
{
	internal class PlayerInputController : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent tempEvent_EndCurrentAct;

		private CharacterMovement _characterMovement;

		[SerializeField]
		private IReplayStorage storage;

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


		private void Update()
		{

			_characterMovement.Rotate(_sideToSideCharacterRotation);
			cameraController.Rotate(_upDownCameraRotation);
		}

		private void FixedUpdate()
		{

			_characterMovement.MoveForward(_moveInput);
		}

		private void Awake()
		{
			_characterMovement = GetComponent<CharacterMovement>();
		}

		private void OnEnable()
		{
			SetCursorActive(true);
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
		}

		private void OnStartNewAct()
		{
			if (!_inputEnabled) return;
			tempEvent_EndCurrentAct.Invoke();
		}

		private void OnSpawn()
		{
			Clone newClone = FindObjectOfType<CharacterSpawner>().SpawnClone(); // Really messy.
		}

		// [UsedImplicitly]
		// private void OnIncreaseTimescale()
		// {
		// Time.timeScale += 1;
		// Logging.instance.Log("Increasing timescale. Now at: " + Time.timeScale);
		// }

		// [UsedImplicitly]
		// private void OnDecreaseTimescale()
		// {
		// Time.timeScale -= 1;
		// Logging.instance.Log("Decreasing timescale. Now at: " + Time.timeScale);
		// }

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
			LockCursor(toggle);
			_inputEnabled = toggle;
		}

		private static void LockCursor(bool locked)
		{
			Cursor.lockState = locked
				? CursorLockMode.Locked
				: CursorLockMode.None;
		}
	}
}