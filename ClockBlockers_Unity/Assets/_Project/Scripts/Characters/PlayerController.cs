using System;
using System.Collections;
using System.Collections.Generic;

using ClockBlockers.Actions;
using ClockBlockers.GameControllers;
using ClockBlockers.Utility;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.InputSystem;



namespace ClockBlockers.Characters
{
	public partial class PlayerController : BaseController
	{
		protected override void Awake()
		{
			base.Awake();
			CurrentFrameActions = new LinkedList<Tuple<Actions.Actions, float[]>>();
		}

		protected override void FixedUpdate()
		{
			MoveCharacterByInput();

			SaveActionsThisFrame();

			base.FixedUpdate();
		}

		private void Update()
		{
			RotateCharacter(SideToSideCharacterRotation);
			RotateCamera(UpDowncameraRotation);
		}


		private void SaveActionsThisFrame()
		{
			if (!RecordActions) return;
			foreach ((Actions.Actions actions, float[] parameter) in CurrentFrameActions)
			{
				var newAction = new CharacterAction
				{
					action = actions, parameter = parameter, time = Time.fixedTime - SpawnTime
				};
				actionStorage.AddActionToUpdatableList(newAction);
			}

			CurrentFrameActions.Clear();
		}

		private void SaveAction(Actions.Actions action, float[] parameters)
		{
			CurrentFrameActions.AddLast(Tuple.Create(action, parameters));

			if (DebugLogEveryAction)
			{
				Logging.Log("Time: " + Time.time + ". Function: " + action + ". Parameters:" + parameters, this);
			}
		}

		private void SaveAction(Actions.Actions action, float parameter)
		{
			float[] arrayedParameter = {parameter};
			SaveAction(action, arrayedParameter);
		}

		private void SaveAction(Actions.Actions action)
		{
			SaveAction(action, new float[0]);
		}

		protected override void RotateCharacter(float rotation)
		{
			if (Mathf.Abs(rotation) < MinInputValue) return;

			float roundedFloat = rotation.Round(GameController.Instance.FloatingPointPrecision);

			SaveAction(Actions.Actions.RotateCharacter, roundedFloat);
			base.RotateCharacter(roundedFloat);
		}

		protected override void RotateCamera(float rotation)
		{
			if (Mathf.Abs(rotation) < MinInputValue) return;

			float roundedFloat = rotation.Round(GameController.Instance.FloatingPointPrecision);

			SaveAction(Actions.Actions.RotateCamera, roundedFloat);
			base.RotateCamera(roundedFloat);
		}

		private void MoveCharacterByInput()
		{
			// If no input, magnitude = 0. I don't want it to record every frame for all eternity. Only when moving.
			if (MovementInput.magnitude < MinInputValue) return;

			Vector2 timeAdjustedInput = MovementInput * Time.fixedDeltaTime;
			MoveCharacterForward(timeAdjustedInput.ToFloatArray());
		}

		protected override void MoveCharacterForward(float[] vector)
		{
			float[] roundedScaledVector = vector.Round().Scale(GameController.Instance.FloatingPointPrecision);

			SaveAction(Actions.Actions.Move, roundedScaledVector);
			base.MoveCharacterForward(roundedScaledVector);
		}

		protected override void AttemptToJump()
		{
			SaveAction(Actions.Actions.Jump);
			base.AttemptToJump();
		}

		protected override void AttemptToShoot()
		{
			SaveAction(Actions.Actions.Shoot);
			base.AttemptToShoot();
		}

		protected override void SpawnReplay()
		{
			if (EnableRecursiveReplays) SaveAction(Actions.Actions.SpawnReplay);

			// HACK: BAD PRACTICE ALERT!
			SaveActionsThisFrame();

			base.SpawnReplay();
		}

		private void SpawnMultipleReplays()
		{
			actionStorage.GameActions.ForEach(action =>
			{
				SpawnReplay(action);
			});
		}

		private void ResetCharacter()
		{
			StopAllCoroutines();

			moveVector = Vector3.zero;
			UpDowncameraRotation = 0;
			SideToSideCharacterRotation = 0;

			actionStorage.ResetUpdatableList();

			SpawnTime = Time.fixedTime;
			transform.SetPositionAndRotation(StartPos, StartRot);
		}

		private void StartNewRound()
		{
			StoreActionsToList();

			GameController.ClearClones();

			SpawnMultipleReplays();

			ResetCharacter();
		}

		private void StoreActionsToList()
		{
			actionStorage.PushRoundDataToList();
		}


		private void SaveActionsToFile()
		{
			DataManipulation.SaveActions(actionStorage.NewlyAddedActions);
		}

		private void LoadActionsFromFile()
		{
			CharacterAction[] actions = DataManipulation.LoadActions();
			SpawnReplay(actions);
		}
	}

	public partial class PlayerController
	{
		private static float MinInputValue { get; } = 0.001f;
		private LinkedList<Tuple<Actions.Actions, float[]>> CurrentFrameActions { get; set; }

		[field: Header("Setup Variables")]
		private Vector2 MovementInput { get; set; }

		private float UpDowncameraRotation { get; set; }
		private float SideToSideCharacterRotation { get; set; }

		private float VerticalMouseSensitivity
		{
			get => verticalMouseSensitivity;
		}

		private float HorizontalMouseSensitivity
		{
			get => horizontalMouseSensitivity;
		}

		[SerializeField]
		[Header("Player Setting")]
		private float verticalMouseSensitivity;

		[SerializeField]
		private float horizontalMouseSensitivity;

		[UsedImplicitly]
		private void OnLook(InputValue ctx)
		{
			var value = ctx.Get<Vector2>();

			SideToSideCharacterRotation = value.x * HorizontalMouseSensitivity * Time.deltaTime;
			UpDowncameraRotation = value.y * VerticalMouseSensitivity * Time.deltaTime;
		}

		[UsedImplicitly]
		private void OnMovement(InputValue ctx)
		{
			MovementInput = ctx.Get<Vector2>();
		}

		[UsedImplicitly]
		private void OnSpawn()
		{
			StartCoroutine(Co_SpawnReplay());
		}

		[UsedImplicitly]
		private void OnJump()
		{
			StartCoroutine(Co_AttemptToJump());
		}

		[UsedImplicitly]
		private void OnShoot(InputValue ctx)
		{
			if (ctx.isPressed) StartCoroutine(Co_AttemptToShoot());
		}

		[UsedImplicitly]
		private void OnClearClones()
		{
			GameController.ClearClones();
		}

		[UsedImplicitly]
		private void OnIncreaseTimescale()
		{
			Time.timeScale += 1;
			Logging.Log("Increasing timescale. Now at: " + Time.timeScale);
		}

		[UsedImplicitly]
		private void OnDecreaseTimescale()
		{
			Time.timeScale -= 1;
			Logging.Log("Decreasing timescale. Now at: " + Time.timeScale);
		}

		[UsedImplicitly]
		private void OnSaveCharacterActions()
		{
			SaveActionsToFile();
		}

		[UsedImplicitly]
		private void OnLoadCharacterActions()
		{
			LoadActionsFromFile();
		}

		[UsedImplicitly]
		private void OnResetRound()
		{
			StartNewRound();
		}

		[UsedImplicitly]
		private void OnToggleCursor()
		{
			GameController.ToggleCursorMode();
		}

		private IEnumerator Co_AttemptToJump()
		{
			yield return new WaitForFixedUpdate();
			AttemptToJump();
		}

		private IEnumerator Co_AttemptToShoot()
		{
			yield return new WaitForFixedUpdate();
			AttemptToShoot();
		}

		private IEnumerator Co_SpawnReplay()
		{
			yield return new WaitForFixedUpdate();

			SpawnReplay();
		}
	}
}