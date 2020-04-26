using System;
using System.Collections;
using System.Collections.Generic;

using ClockBlockers.Characters;
using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.Weapons;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayRunner
{
	public class ActionReplayRunner : MonoBehaviour
	{
		private Character _character;

		[SerializeField]
		private Gun gun;
		
		[NonSerialized]
		public List<CharacterAction> replays;
		
		private int _remainingActions;

		private void Awake()
		{
			_character = GetComponent<Character>();

			_character.onKilled += End;
		}


		// This needs to be made more modular.
		private void RunAction(CharacterAction characterAction)
		{
			_remainingActions--;

			Actions action = characterAction.action;
			float[] parameter = characterAction.parameter;

			switch (action)
			{
				// case Actions.Move:
				// 	// MoveAction?.Invoke(parameter);
				// 	break;
				//
				// case Actions.RotateCharacter:
				// 	// RotateCharacterAction?.Invoke(parameter);
				// 	break;
				//
				// case Actions.RotateCamera:
				// 	// RotateCameraAction?.Invoke(parameter);
				// 	break;

				case Actions.Shoot:
					gun.PullTrigger();
					// ShootAction?.Invoke(parameter);
					break;

				case Actions.SpawnReplay:
					// SpawnReplayAction?.Invoke(parameter);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(action), action, null);
			}

			if (_remainingActions == 0)
			{
				// CompletedAllActions?.Invoke();
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
			foreach (CharacterAction characterAction in replays)
			{
				EngageAction(characterAction);
			}
			_remainingActions += replays.Count;
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