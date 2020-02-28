using ClockBlockers.Characters;

using UnityEngine;


namespace ClockBlockers.GameControllers {
	public class CharacterSpawner : MonoBehaviour
	{

		[SerializeField]
		private Character playerPrefab;

		[SerializeField]
		private Character clonePrefab;
		
		public Player SpawnPlayer()
		{
			return SpawnCharacter(playerPrefab).GetComponent<Player>();
		}

		public Clone SpawnClone()
		{
			return SpawnCharacter(clonePrefab).GetComponent<Clone>();
		}
		
		
		private static Character SpawnCharacter(Character character)
		{
			return Instantiate(character);
		}
	}
}