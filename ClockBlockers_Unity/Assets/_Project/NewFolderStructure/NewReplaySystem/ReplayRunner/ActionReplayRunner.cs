using System;
using System.Collections;

using UnityEngine;


namespace ClockBlockers.NewReplaySystem.ReplayRunner
{
	public class ActionReplayRunner : MonoBehaviour, IReplayRunner
	{
		internal delegate void ActionDelegate(float[] value);

		private int _remainingActions;

		internal ActionDelegate moveAction;
		internal ActionDelegate rotateCharacterAction;
		internal ActionDelegate rotateCameraAction;
		internal ActionDelegate jumpAction;
		internal ActionDelegate shootAction;
		internal ActionDelegate spawnReplayAction;

		internal Action completedAllActions;


		private void RunAction(CharacterAction characterAction)
		{
			_remainingActions--;

			Actions action = characterAction.action;
			float[] parameter = characterAction.parameter;

			switch (action)
			{
				case Actions.Move:
					moveAction?.Invoke(parameter);
					break;

				case Actions.RotateCharacter:
					rotateCharacterAction?.Invoke(parameter);
					break;

				case Actions.RotateCamera:
					rotateCameraAction?.Invoke(parameter);
					break;

				case Actions.Jump:
					jumpAction?.Invoke(parameter);
					break;

				case Actions.Shoot:
					shootAction?.Invoke(parameter);
					break;

				case Actions.SpawnReplay:
					spawnReplayAction?.Invoke(parameter);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(action), action, null);
			}

			if (_remainingActions == 0)
			{
				completedAllActions();
			}
		}

		// Adding MonoBehaviour (and therefore requiring to add this class as a GameObject component) just to be able to call Coroutines; Is that worth it?
		// is this a job (pun intended) for the Job system?
		// I might be looking to much into this.. My FPS camera doesn't even behave correctly :lul:
		internal void EngageAction(CharacterAction characterAction)
		{
			StartCoroutine(Co_Action(characterAction));
		}

		internal void EngageAllActions(CharacterAction[] characterActions)
		{
			foreach (CharacterAction characterAction in characterActions)
			{
				EngageAction(characterAction);
			}

			_remainingActions += characterActions.Length;
		}

		private IEnumerator Co_Action(CharacterAction characterAction)
		{
			yield return new WaitForSeconds(characterAction.time - Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
			RunAction(characterAction);
		}

		public void StopRunning()
		{
			StopAllCoroutines();
		}

		public void StartRunning()
		{
			throw new NotImplementedException();
		}
	}
}