using System;


namespace ClockBlockers.NewReplaySystem
{
	[Serializable]
	public struct CharacterAction
	{
		public Actions action;
		public float[] parameter;
		public float time;

		public CharacterAction(Actions action, float[] parameter, float time)
		{
			this.action = action;
			this.parameter = parameter;
			this.time = time;
		}

		public CharacterAction(Actions action, float parameter, float time) 
			: this(action, new[] {parameter}, time)
		{
			
		}

		public CharacterAction(Actions action, float time) : this(action, new float[0], time)
		{
			
		}

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