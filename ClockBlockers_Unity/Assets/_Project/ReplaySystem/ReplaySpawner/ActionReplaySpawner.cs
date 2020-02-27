using System.Collections.Generic;

using ClockBlockers.Characters.Scripts;
using ClockBlockers.GameControllers;
using ClockBlockers.ReplaySystem.ReplayStorage;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplaySpawner {
	public class ActionReplaySpawner : MonoBehaviour, IReplaySpawner
	{
		private ActionReplayStorage _actionReplayStorage;
		
		private GameController _gameController;
		
		[SerializeField]
		private Character clonePrefab;

		private void Awake()
		{
			// DILEMMA: I do not want use "FindObjectOfType" on GameController, because I don't want the singleton pattern, but I also can't assign GameController via the Interface.
			// This will hopefully be fixed by the time I refactor the GameController into smaller parts.

			_gameController = FindObjectOfType<GameController>();
			_actionReplayStorage = GetComponent<ActionReplayStorage>();
		}
		private void SpawnClone(Vector3 position, Quaternion rotation, CharacterAction[] actions)
		{
			// Character newCloneController = _gameController.SpawnCharacter(clonePrefab, position, rotation);

			// newCloneController.Cam.transform.localRotation = Quaternion.identity;

			// newCloneController.replayStorage.SetActReplayData(actions);
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

	public interface IReplaySpawner {
		// They all need to know what to spawn
		void SpawnLatestReplay();
		void SpawnAllReplays();
	}
}