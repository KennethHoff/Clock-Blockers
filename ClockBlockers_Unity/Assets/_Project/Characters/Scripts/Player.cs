using System.Collections;

using ClockBlockers.Input;
using ClockBlockers.NewReplaySystem;
using ClockBlockers.NewReplaySystem.ReplaySaver;
using ClockBlockers.Utility;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.InputSystem;


namespace ClockBlockers.Characters.Scripts
{
	public class Player : Character
	{
		private static float MinInputValue { get; } = 0.001f;

		private IReplaySaver _replaySaver;

		private InputController _inputController;

		protected override void Awake()
		{
			base.Awake();

			_replaySaver = GetComponent<IReplaySaver>();
			if (_replaySaver == null)
			{
				// ActionReplaySaver relies on ActionReplayStorage, so I have to swap it out if I want to use that Saver.
				Logging.LogWarning("No Replay Saver!");
			}
			_inputController = GetComponent<InputController>();
			if (_inputController == null) Logging.LogWarning("No input controller on " + name, this);
			_inputController.gameController = gameController;

			waitForFixedFrame = new WaitForFixedUpdate();
		}

		protected override void Start()
		{
			base.Start();
			_inputController.gameController = gameController;
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


		protected override void RotateCharacter(float rotation)
		{
			if (Mathf.Abs(rotation) < MinInputValue) return;

			float roundedFloat = rotation.Round(gameController.FloatingPointPrecision);

			_replaySaver.SaveAction(CreateCharacterAction(Actions.RotateCharacter, roundedFloat));
			base.RotateCharacter(roundedFloat);
		}

		protected override void RotateCamera(float rotation)
		{
			if (Mathf.Abs(rotation) < MinInputValue) return;

			float roundedFloat = rotation.Round(gameController.FloatingPointPrecision);

			_replaySaver.SaveAction(CreateCharacterAction(Actions.RotateCamera, roundedFloat));
			base.RotateCamera(roundedFloat);
		}

		private void MoveCharacterByInput()
		{
			// If no input, magnitude = 0. I don't want it to record every frame for all eternity. Only when moving.
			if (_inputController.MovementInput.magnitude < MinInputValue) return;

			Vector2 timeAdjustedInput = _inputController.MovementInput * Time.fixedDeltaTime;
			MoveCharacterForward(timeAdjustedInput.ToFloatArray());
		}

		private CharacterAction CreateCharacterAction(Actions action, float[] parameter)
		{
			return new CharacterAction(action, parameter, gameController.FixedTimeSinceActStarted);
		}
		private CharacterAction CreateCharacterAction(Actions action, float parameter)
		{
			return new CharacterAction(action, parameter, gameController.FixedTimeSinceActStarted);
		}
		private CharacterAction CreateCharacterAction(Actions action)
		{
			return new CharacterAction(action, gameController.FixedTimeSinceActStarted);
		}
		
		protected override void MoveCharacterForward(float[] vector)
		{
			float[] roundedScaledVector = vector.Round().Scale(gameController.FloatingPointPrecision);

			_replaySaver.SaveAction(CreateCharacterAction(Actions.Move, vector));
			base.MoveCharacterForward(roundedScaledVector);
		}

		protected override void AttemptToShoot()
		{
			_replaySaver.SaveAction(CreateCharacterAction(Actions.Shoot));
			base.AttemptToShoot();
		}

		protected override void SpawnReplay()
		{
			_replaySaver.SaveAction(CreateCharacterAction(Actions.SpawnReplay));
			base.SpawnReplay();
		}

		private void ResetCharacter()
		{
			StopAllCoroutines();

			_inputController.UpDownCameraRotation = 0;
			_inputController.SideToSideCharacterRotation = 0;

			Cam.transform.rotation = camStartRot;

			replayStorage.ClearStorageForThisAct();

			characterController.enabled = false;
			transform.SetPositionAndRotation(startPos, startRot);
			characterController.enabled = true;
		}

		private IEnumerator Co_AttemptToShoot()
		{
			yield return waitForFixedFrame;
			AttemptToShoot();
		}

		private IEnumerator Co_SpawnReplay()
		{
			yield return waitForFixedFrame;

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

		[UsedImplicitly]
		private void OnClearClones()
		{
			gameController.ClearClones();
		}

		protected override void OnActEnded()
		{
			base.OnActEnded();

			_replaySaver.PushActDataToRound();
			ResetCharacter();

			_inputController.Reset();
		}

		protected override void OnNewActStart()
		{
			base.OnNewActStart();

			replaySpawner.SpawnAllReplays();
		}
	}
}