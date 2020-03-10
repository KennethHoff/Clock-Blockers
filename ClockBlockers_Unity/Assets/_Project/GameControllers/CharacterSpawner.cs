using ClockBlockers.Characters;

using UnityEngine;


namespace ClockBlockers.GameControllers {
	public class CharacterSpawner : MonoBehaviour
	{

		[SerializeField]
		private Character playerPrefab;

		[SerializeField]
		private Character clonePrefab;
		
		public Character SpawnPlayer()
		{
			return SpawnCharacter(playerPrefab);
		}

		public Character SpawnClone()
		{
			return SpawnCharacter(clonePrefab);
		}
		
		
		private static Character SpawnCharacter(Character character)
		{
			return Instantiate(character);
		}
	}
}