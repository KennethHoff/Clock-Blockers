using System;

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

		[NonSerialized]
		private PathfindingGrid _grid;

		public Character SpawnPlayer()
		{
			Character newPlayer = SpawnCharacter(playerPrefab);
			return newPlayer;
		}

		public void Inject(PathfindingGrid grid)
		{
			_grid = grid;
		}

		public Character SpawnClone()
		{
			Character newClone = SpawnCharacter(clonePrefab);
			var aiPathfinder = newClone.GetComponent<AiPathfinder>();

			if (aiPathfinder != null)
			{
				aiPathfinder.Inject(_grid.pathfindingManager, _grid);
			}
			else
			{
				Logging.LogWarning("Clone has no Pathfinder");
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