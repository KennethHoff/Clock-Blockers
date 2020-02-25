using ClockBlockers.GameControllers;
using ClockBlockers.Utility;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.InputSystem;


namespace ClockBlockers.Input
{
	internal class InputController : MonoBehaviour
	{
		[HideInInspector]
		public GameController gameController;

		public bool HoldingMouseButton { get; internal set; }

		[SerializeField]
		private float verticalMouseSensitivity;

		[SerializeField]
		private float horizontalMouseSensitivity;

		public float UpDownCameraRotation { get; set; }

		public float SideToSideCharacterRotation { get; set; }

		[UsedImplicitly]
		private void OnLook(InputValue ctx)
		{
			var value = ctx.Get<Vector2>();

			SideToSideCharacterRotation = value.x * horizontalMouseSensitivity * Time.deltaTime;
			UpDownCameraRotation = value.y * verticalMouseSensitivity * Time.deltaTime;
		}


		[UsedImplicitly]
		private void OnMovement(InputValue ctx)
		{
			MovementInput = ctx.Get<Vector2>();
		}

		public Vector2 MovementInput { get; set; }

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
		private void OnToggleCursor()
		{
			GameController.ToggleCursorMode();
		}


		[UsedImplicitly]
		private void OnStartNewAct()
		{
			gameController.EndCurrentAct();
			gameController.StartNewAct();
		}

		public void Reset()
		{
			UpDownCameraRotation = 0;
			SideToSideCharacterRotation = 0;
			HoldingMouseButton = false;
		}
	}
}