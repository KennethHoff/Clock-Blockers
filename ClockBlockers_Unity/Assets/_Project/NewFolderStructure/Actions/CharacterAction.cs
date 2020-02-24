using System;


namespace ClockBlockers.Actions
{
	[Serializable]
	public struct CharacterAction
	{
		public Actions action;
		public float[] parameter;
		public float time;
	}

	public enum Actions
	{
		Move,
		RotateCharacter, // Read under
		RotateCamera, // Should probably be merged into a single one.
		Jump,
		Shoot,
		SpawnReplay
	}
}