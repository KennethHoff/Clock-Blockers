using System;

using ClockBlockers.DataStructures;
using ClockBlockers.GameControllers;
using ClockBlockers.Utility;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.InputSystem;


namespace ClockBlockers.Input
{
	internal class InputController : MonoBehaviour
	{
		[NonSerialized]
		internal GameController gameController;


		private float VerticalMouseSensitivity
		{
			get => verticalMouseSensitivity.Value;
		}


		private float HorizontalMouseSensitivity
		{
			get => horizontalMouseSensitivity.Value;
		}

		[SerializeField]
		private FloatReference verticalMouseSensitivity;

		[SerializeField]
		private FloatReference horizontalMouseSensitivity;

		public float UpDownCameraRotation { get; set; }

		public float SideToSideCharacterRotation { get; set; }

		[UsedImplicitly]
		private void OnLook(InputValue ctx)
		{
			var value = ctx.Get<Vector2>();

			SideToSideCharacterRotation = value.x * HorizontalMouseSensitivity * Time.fixedDeltaTime;
			UpDownCameraRotation = value.y * VerticalMouseSensitivity * Time.fixedDeltaTime;
		}


		[UsedImplicitly]
		private void OnMovement(InputValue ctx)
		{
			MovementInput = ctx.Get<Vector2>();
		}

		public Vector2 MovementInput { get; private set; }

		[UsedImplicitly]
		private void OnIncreaseTimescale()
		{
			Time.timeScale += 1;
			Logging.instance.Log("Increasing timescale. Now at: " + Time.timeScale);
		}

		[UsedImplicitly]
		private void OnDecreaseTimescale()
		{
			Time.timeScale -= 1;
			Logging.instance.Log("Decreasing timescale. Now at: " + Time.timeScale);
		}


		[UsedImplicitly]
		private void OnToggleCursor()
		{
			ToggleCursorMode();
		}


		// [UsedImplicitly]
		// private void OnStartNewAct()
		// {
		// 	gameController.EndCurrentAct();
		// 	gameController.StartNewAct();
		// }

		public void Reset()
		{
			UpDownCameraRotation = 0;
			SideToSideCharacterRotation = 0;
		}
		

		private static void SetCursorMode(bool locked)
		{
			Cursor.lockState = locked
				? CursorLockMode.Locked
				: CursorLockMode.None;
		}

		public static void ToggleCursorMode()
		{
			SetCursorMode(Cursor.lockState != CursorLockMode.Locked);
		}

	}
}