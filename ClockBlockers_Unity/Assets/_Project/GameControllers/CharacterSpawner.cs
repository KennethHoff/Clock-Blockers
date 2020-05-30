using System;

using ClockBlockers.AI;
using ClockBlockers.Characters;
using ClockBlockers.MapData;
using ClockBlockers.MatchData;
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

		public Character SpawnPlayer(Act act)
		{
			Character newPlayer = SpawnCharacter(playerPrefab, act);
			return newPlayer;
		}

		public void Inject(PathfindingGrid grid)
		{
			_grid = grid;
		}

		public Character SpawnClone(Act act)
		{
			Character newClone = SpawnCharacter(clonePrefab, act);
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

		private static Character SpawnCharacter(Character character, Act act)
		{
			Character newCharacter = Instantiate(character, Vector3.up *10, Quaternion.identity);
			newCharacter.Inject(act);
			return newCharacter;
		}
	}
}