// using System;
//
// using ClockBlockers.GameControllers;
// using ClockBlockers.Utility;
//
// using JetBrains.Annotations;
//
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// namespace ClockBlockers.Input {
// 	public class InputController
// 	{
// 		
// 		[UsedImplicitly]
// 		private void OnLook(InputValue ctx)
// 		{
// 			var value = ctx.Get<Vector2>();
//
// 			SideToSideCharacterRotation = value.x * HorizontalMouseSensitivity * Time.deltaTime;
// 			UpDowncameraRotation = value.y * VerticalMouseSensitivity * Time.deltaTime;
// 		}
//
// 		[UsedImplicitly]
// 		private void OnMovement(InputValue ctx)
// 		{
// 			MovementInput = ctx.Get<Vector2>();
// 		}
//
// 		[UsedImplicitly]
// 		private void OnSpawn()
// 		{
// 			StartCoroutine(Co_SpawnReplay());
// 		}
//
// 		[UsedImplicitly]
// 		private void OnJump()
// 		{
// 			StartCoroutine(Co_AttemptToJump());
// 		}
//
// 		[UsedImplicitly]
// 		private void OnShoot(InputValue ctx)
// 		{
// 			if (ctx.isPressed) StartCoroutine(Co_AttemptToShoot());
// 		}
//
// 		[UsedImplicitly]
// 		private void OnClearClones()
// 		{
// 			GameController.ClearClones();
// 		}
//
// 		[UsedImplicitly]
// 		private void OnIncreaseTimescale()
// 		{
// 			Time.timeScale += 1;
// 			Logging.Log("Increasing timescale. Now at: " + Time.timeScale);
// 		}
//
// 		[UsedImplicitly]
// 		private void OnDecreaseTimescale()
// 		{
// 			Time.timeScale -= 1;
// 			Logging.Log("Decreasing timescale. Now at: " + Time.timeScale);
// 		}
//
// 		[UsedImplicitly]
// 		private void OnSaveCharacterActions()
// 		{
// 			SaveActionsToFile();
// 		}
//
// 		[UsedImplicitly]
// 		private void OnLoadCharacterActions()
// 		{
// 			LoadActionsFromFile();
// 		}
//
// 		[UsedImplicitly]
// 		private void OnResetRound()
// 		{
// 			StartNewRound();
// 		}
//
// 		[UsedImplicitly]
// 		private void OnToggleCursor()
// 		{
// 			GameController.ToggleCursorMode();
// 		}
//
// 	}
// }

