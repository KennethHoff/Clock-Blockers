using System.Collections.Generic;

using ClockBlockers.Characters.Scripts;
using ClockBlockers.GameControllers;
using ClockBlockers.NewReplaySystem.ReplayStorage;

using UnityEngine;


namespace ClockBlockers.NewReplaySystem.ReplaySpawner
{
	public class ActionReplaySpawner : MonoBehaviour, IReplaySpawner
	{

		private ActionReplayStorage _actionReplayStorage;
		
		private GameController _gameController;
		
		[SerializeField]
		private Character clonePrefab;

		private void Awake()
		{
			_gameController = FindObjectOfType<GameController>();
			_actionReplayStorage = GetComponent<ActionReplayStorage>();
		}

		// public ActionReplaySpawner(GameController gameController, Character clonePrefab, ActionReplayStorage actionReplayStorage)
		// {
		// 	_gameController = gameController;
		// 	_clonePrefab = clonePrefab;
		// 	_actionReplayStorage = actionReplayStorage;
		// }

		private void SpawnClone(Vector3 position, Quaternion rotation, CharacterAction[] actions)
		{
			Character newCloneController = GameController.SpawnCharacter(clonePrefab, position, rotation);

			newCloneController.gameController = _gameController;

			newCloneController.Cam.transform.localRotation = Quaternion.identity;

			newCloneController.replayStorage.SetActActions(actions);
		}

		public void SpawnLatestReplay()
		{
			SpawnReplay(_actionReplayStorage.GetCurrentAct());
		}

		private void SpawnReplay(CharacterAction[] actions)
		{
			SpawnClone(Vector3.up, Quaternion.identity, actions);
		}

		public void SpawnAllReplays()
		{
			List<CharacterAction[]> allRounds = _actionReplayStorage.GetAllActs();
			
			// foreach (CharacterAction[] t in allRounds)
			// {
			// 	SpawnReplay(t);
			// }
			
			allRounds.ForEach(SpawnReplay);
		}
	}
}