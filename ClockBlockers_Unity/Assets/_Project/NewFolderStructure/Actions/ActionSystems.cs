using ClockBlockers.Characters;
using ClockBlockers.GameControllers;

using UnityEngine;


namespace ClockBlockers.Actions
{
	public class ActionSystems : MonoBehaviour
	{
		public static GameObject SpawnReplay(GameObject prefab, Vector3 spawnPos, Quaternion spawnRot, CharacterAction[] actions)
		{
			return SpawnClone(prefab, spawnPos, spawnRot, actions);
		}

		public static GameObject SpawnClone(GameObject prefab, Vector3 position, Quaternion rotation, CharacterAction[] actions)
		{
			GameObject newClone = Instantiate(prefab, position, rotation, GameController.instance.CloneParent);

			var newCloneController = newClone.GetComponent<CloneController>();

			newCloneController.Cam.transform.localRotation = Quaternion.identity;

			newCloneController.actionStorage = new ActionStorage(actionArray: actions);

			return newClone;
		}
	}
}