using System.Collections.Generic;

using ClockBlockers.Characters;
using ClockBlockers.ReplaySystem.ReplayStorage;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplaySpawner {
	public class ActionReplaySpawner : MonoBehaviour, IReplaySpawner
	{
		private ActionReplayStorage _actionReplayStorage;
		
		private void Awake()
		{
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
}