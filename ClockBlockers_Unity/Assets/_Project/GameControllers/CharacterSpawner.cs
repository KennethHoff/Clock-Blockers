using ClockBlockers.AI;
using ClockBlockers.Characters;
using ClockBlockers.MapData;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.GameControllers {
	[BurstCompile]
	public class CharacterSpawner : MonoBehaviour
	{

		[SerializeField]
		private Character playerPrefab = null;

		[SerializeField]
		private Character clonePrefab = null;

		public PathfindingGrid grid;

		public Character SpawnPlayer()
		{
			Character newPlayer = SpawnCharacter(playerPrefab);
			return newPlayer;
		}

		public Character SpawnClone()
		{
			Character newClone = SpawnCharacter(clonePrefab);
			var aiPathfinder = newClone.GetComponent<AiPathfinder>();
			
			if (aiPathfinder == null)
			{
				Logging.LogWarning("Clone has no PathFinder");
			}
			else
			{
				aiPathfinder.pathfindingManager = grid.pathfindingManager;
			}

			return newClone;
		}
		
		
		private static Character SpawnCharacter(Character character)
		{
			Character newCharacter = Instantiate(character, Vector3.up *10, Quaternion.identity);
			return newCharacter;
		}
	}
}