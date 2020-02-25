using System.Collections.Generic;

using ClockBlockers.Characters;
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

		public Character SpawnReplay(Vector3 spawnPos, Quaternion spawnRot, CharacterAction[] actions)
		{
			return SpawnClone(spawnPos, spawnRot, actions);
		}

		private Character SpawnClone(Vector3 position, Quaternion rotation, CharacterAction[] actions)
		{
			Character newCloneController = _gameController.SpawnCharacter(clonePrefab, position, rotation);

			newCloneController.gameController = _gameController;

			newCloneController.Cam.transform.localRotation = Quaternion.identity;

			newCloneController.replayStorage.SetActActions(actions);
			return newCloneController;
		}

		public void SpawnLatestReplay()
		{
			SpawnReplay(_actionReplayStorage.GetCurrentAct());
		}

		public void SpawnReplay(CharacterAction[] actions)
		{
			SpawnClone(Vector3.up, Quaternion.identity, actions);
		}

		public void SpawnAllReplays()
		{
			List<CharacterAction[]> allRounds = _actionReplayStorage.GetAllActs();
			for (var i = 0; i < allRounds.Count; i++)
			{
				SpawnReplay(allRounds[i]);
			}
		}
	}
}