using System;
using System.Collections;

using ClockBlockers.Actions;

using UnityEngine;



namespace ClockBlockers.Characters
{
	public class CloneController : BaseController
	{
		private float RelativeSpeed
		{
			get => relativeSpeed;
		}

		[SerializeField]
		private float relativeSpeed;


		protected override void Start()
		{
			base.Start();
			EngageAllActions();
		}

		private void EngageAllActions()
		{
			foreach (CharacterAction characterAction in actionStorage.ReplayActions)
			{
				EngageAction(characterAction);
			}
		}

		private void RunAction(CharacterAction characterAction)
		{
			Actions.Actions action = characterAction.action;
			float[] parameter = characterAction.parameter;

			switch (action)
			{
				case Actions.Actions.Move:
					Action_MoveCharacterNew(parameter);
					break;

				case Actions.Actions.RotateCharacter:
					Action_RotateCharacterNew(parameter);
					break;

				case Actions.Actions.RotateCamera:
					Action_RotateCameraNew(parameter);
					break;

				case Actions.Actions.Jump:
					Action_AttemptToJumpNew(parameter);
					break;

				case Actions.Actions.Shoot:
					Action_AttemptToShootNew(parameter);
					break;

				case Actions.Actions.SpawnReplay:
					Action_SpawnReplayNew(parameter);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(action), action, null);
			}
		}

		private void EngageAction(CharacterAction characterAction)
		{
			StartCoroutine(Co_Action_Generic(characterAction));
		}

		private IEnumerator Co_Action_Generic(CharacterAction characterAction)
		{
			yield return new WaitForSeconds(characterAction.time - Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
			RunAction(characterAction);
		}

		private void Action_MoveCharacterNew(float[] value)
		{
			float[] move = {value[0], value[1]};
			MoveCharacterForward(move);
		}

		private void Action_RotateCharacterNew(float[] value)
		{
			float rotation = value[0];
			RotateCharacter(rotation);
		}

		private void Action_RotateCameraNew(float[] value)
		{
			float rotation = value[0];
			RotateCamera(rotation);
		}

		private void Action_AttemptToJumpNew(float[] value)
		{
			AttemptToJump();
		}

		private void Action_AttemptToShootNew(float[] value)
		{
			AttemptToShoot();
		}

		private void Action_SpawnReplayNew(float[] value)
		{
			SpawnReplay();
		}
	}
}