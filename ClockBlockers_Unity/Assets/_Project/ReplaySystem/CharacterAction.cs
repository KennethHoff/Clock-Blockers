using System;

using Unity.Burst;


namespace ClockBlockers.ReplaySystem
{
	[Serializable][BurstCompile]
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

	// DONE: Remove Move, RotateCharacter, and RotateCamera
	// TODO: Re-implement this
	public enum Actions
	{
		Shoot,
		SpawnReplay
	}
}