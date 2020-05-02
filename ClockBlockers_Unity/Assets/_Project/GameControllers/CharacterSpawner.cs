using ClockBlockers.Characters;

using UnityEngine;


namespace ClockBlockers.GameControllers {
	public class CharacterSpawner : MonoBehaviour
	{

		[SerializeField]
		private Character playerPrefab = null;

		[SerializeField]
		private Character clonePrefab = null;
		
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