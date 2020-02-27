using System;
using System.Collections;

using ClockBlockers.ReplaySystem.ReplayStorage;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayRunner
{
	public class ActionReplayRunner : MonoBehaviour, IReplayRunner
	{

		private ActionReplayStorage _actionReplayStorage;
		internal delegate void ActionDelegate(float[] value);

		private int _remainingActions;

		internal event ActionDelegate MoveAction;
		internal event ActionDelegate RotateCharacterAction;
		internal event ActionDelegate RotateCameraAction;
		// internal ActionDelegate jumpAction;
		internal event ActionDelegate ShootAction;
		internal event ActionDelegate SpawnReplayAction;

		internal event Action CompletedAllActions;

		private void Awake()
		{
			_actionReplayStorage = GetComponent<ActionReplayStorage>();
		}

		private void RunAction(CharacterAction characterAction)
		{
			_remainingActions--;

			Actions action = characterAction.action;
			float[] parameter = characterAction.parameter;

			switch (action)
			{
				case Actions.Move:
					MoveAction?.Invoke(parameter);
					break;

				case Actions.RotateCharacter:
					RotateCharacterAction?.Invoke(parameter);
					break;

				case Actions.RotateCamera:
					RotateCameraAction?.Invoke(parameter);
					break;

				case Actions.Shoot:
					ShootAction?.Invoke(parameter);
					break;

				case Actions.SpawnReplay:
					SpawnReplayAction?.Invoke(parameter);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(action), action, null);
			}

			if (_remainingActions == 0)
			{
				CompletedAllActions?.Invoke();
			}
		}

		// Adding MonoBehaviour (and therefore requiring to add this class as a GameObject component) just to be able to call Coroutines; Is that worth it?
		// [*Edit:* No longer just because of that. Huzzah! :p]
		
		// is this a job (pun intended) for the Job system?
		// I might be looking to much into this.. My FPS camera doesn't even behave correctly :lul:
		private void EngageAction(CharacterAction characterAction)
		{
			StartCoroutine(Co_Action(characterAction));
		}

		private void EngageAllActions()
		{
			foreach (CharacterAction characterAction in _actionReplayStorage.CurrentActNpcActions)
			{
				EngageAction(characterAction);
			}

			_remainingActions += _actionReplayStorage.CurrentActNpcActions.Length;
		}

		private IEnumerator Co_Action(CharacterAction characterAction)
		{
			yield return new WaitForSeconds(characterAction.time - Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
			RunAction(characterAction);
		}

		public void End()
		{
			StopAllCoroutines();
		}

		public void Begin()
		{
			EngageAllActions();
		}
	}
}