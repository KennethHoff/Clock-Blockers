using System;

using ClockBlockers.GameControllers;
using ClockBlockers.NewReplaySystem.ReplayStorage;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.NewReplaySystem.ReplaySaver
{
	public class ActionReplaySaver : MonoBehaviour, IReplaySaver
	{
		private ActionReplayStorage _actionReplayStorage;
		private GameController _gameController;

		
		[SerializeField]
		// ReSharper disable once RedundantDefaultMemberInitializer
		private bool debugLogEveryAction = false;

		// public ActionReplaySaver(ActionReplayStorage actionReplayStorage, bool debugLogEveryAction, GameController gameController)
		// {
		// 	_actionReplayStorage = actionReplayStorage;
		// 	_debugLogEveryAction = debugLogEveryAction;
		// 	_gameController = gameController;
		// }


		private void Awake()
		{
			_gameController = FindObjectOfType<GameController>();
			_actionReplayStorage = GetComponent<ActionReplayStorage>();
		}

		public void SaveAction(Actions action, float[] parameters)
		{
			if (debugLogEveryAction)
			{
				Logging.Log("Time: " + Time.time + ". Function: " + action + ". Parameters:" + parameters);
			}

			float currentTime = _gameController.TimeWhenActStarted;
			var newCharacterAction = new CharacterAction(action, parameters, currentTime);
			_actionReplayStorage.AddActionToAct(newCharacterAction);
		}

		public void StoreActionsToList()
		{
			_actionReplayStorage.PushActDataToRound();
		}

		public void SaveAction(CharacterAction characterAction)
		{
			if (debugLogEveryAction)
			{
				Logging.Log(characterAction);
			}

			_actionReplayStorage.AddActionToAct(characterAction);
		}

		public void PushActDataToRound()
		{
			throw new NotImplementedException();
		}
	}
}