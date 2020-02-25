using ClockBlockers.NewReplaySystem.ReplayStorage;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.NewReplaySystem.ReplaySaver
{
	public class ActionReplaySaver : MonoBehaviour, IReplaySaver
	{
		private ActionReplayStorage _actionReplayStorage;


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
			_actionReplayStorage = GetComponent<ActionReplayStorage>();
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
			_actionReplayStorage.PushActDataToRound();
		}
	}
}