using System;

using ClockBlockers.Characters.Scripts;
using ClockBlockers.Events;

using UnityEngine;


namespace ClockBlockers.GameControllers {
	public class CharacterSpawner : MonoBehaviour
	{
		[SerializeField]
		private Character playerPrefab;

		[SerializeField]
		private Character clonePrefab;
		private Character SpawnCharacter(Character character, Vector3 position, Quaternion rotation)
		{
			return Instantiate(character, position, rotation);
		}
		
		public void SpawnPlayer()
		{
			SpawnCharacter(playerPrefab, Vector3.one, Quaternion.identity);
		}

		public Clone SpawnClone()
		{
			return SpawnCharacter(clonePrefab, Vector3.one, Quaternion.identity).GetComponent<Clone>();
		}
	}
}