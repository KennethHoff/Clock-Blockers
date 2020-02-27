using System.Collections;

using ClockBlockers.Input;
using ClockBlockers.ReplaySystem;
using ClockBlockers.Utility;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.InputSystem;


namespace ClockBlockers.Characters.Scripts
{
	[RequireComponent(typeof(Character))]
	public class Player : MonoBehaviour
	{
		private static float MinInputValue { get; } = 0.001f;



		private Character _character;
		private InputController _inputController;
		private WaitForFixedUpdate _waitForFixedFrame;

		protected void Awake()
		{
			_character = GetComponent<Character>();
			_inputController = GetComponent<InputController>();
			if (_inputController == null) Logging.instance.LogWarning("No input controller on " + name, this);

			_waitForFixedFrame = new WaitForFixedUpdate();
		}

		protected void Start()
		{
			_inputController.gameController = _character.gameController;
		}


		protected void FixedUpdate()
		{
			MoveCharacterByInput();
		}

		private void Update()
		{
			RotateCharacter(_inputController.SideToSideCharacterRotation);
			RotateCamera(_inputController.UpDownCameraRotation);
		}


		private void RotateCharacter(float rotation)
		{
			if (Mathf.Abs(rotation) < MinInputValue) return;

			_character.replayStorage.SaveAction(Actions.RotateCharacter, rotation);
			_character.RotateCharacter(rotation);
		}

		private void RotateCamera(float rotation)
		{
			if (Mathf.Abs(rotation) < MinInputValue) return;

			_character.replayStorage.SaveAction(Actions.RotateCamera, rotation);
			_character.RotateCamera(rotation);
		}

		private void MoveCharacterByInput()
		{
			// If no input, magnitude = 0. I don't want it to record every frame for all eternity. Only when moving.
			if (_inputController.MovementInput.magnitude < MinInputValue) return;

			Vector2 timeAdjustedInput = _inputController.MovementInput * Time.fixedDeltaTime;
			MoveCharacterForward(timeAdjustedInput.ToFloatArray());
		}


		private void MoveCharacterForward(float[] vector)
		{
			_character.replayStorage.SaveAction(Actions.Move, vector);
			_character.MoveCharacterForward(vector);
		}

		private void AttemptToShoot()
		{
			_character.replayStorage.SaveAction(Actions.Shoot);
			_character.AttemptToShoot();
		}

		private void SpawnReplay()
		{
			_character.replayStorage.SaveAction(Actions.SpawnReplay);
			_character.SpawnReplay();
		}

		private void ResetCharacter()
		{
			StopAllCoroutines();

			_inputController.UpDownCameraRotation = 0;
			_inputController.SideToSideCharacterRotation = 0;

			_character.Cam.transform.rotation = _character.CamStartRot;

			_character.replayStorage.ClearStorageForThisAct();

			_character.characterController.enabled = false;
			transform.SetPositionAndRotation(_character.StartPos, _character.StartRot);
			_character.characterController.enabled = true;
		}

		private IEnumerator Co_AttemptToShoot()
		{
			yield return _waitForFixedFrame;
			AttemptToShoot();
		}

		private IEnumerator Co_SpawnReplay()
		{
			yield return _waitForFixedFrame;

			SpawnReplay();
		}

		[UsedImplicitly]
		private void OnSpawn()
		{
			StartCoroutine(Co_SpawnReplay());
		}

		[UsedImplicitly]
		private void OnShoot(InputValue ctx)
		{
			StartCoroutine(Co_AttemptToShoot());
		}

		// protected override void OnActEnded()
		// {
		// 	base.OnActEnded();
		//
		// 	replayStorage.StoreActData();
		// 	ResetCharacter();
		//
		// 	_inputController.Reset();
		// }

		// protected override void OnNewActStart()
		// {
		// 	base.OnNewActStart();
		//
		// 	replaySpawner.SpawnAllReplays();
		// }
	}
}