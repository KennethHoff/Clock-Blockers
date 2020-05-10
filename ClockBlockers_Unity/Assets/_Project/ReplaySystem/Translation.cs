using System;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.ReplaySystem
{
	[Serializable][BurstCompile]
	public struct Translation
	{
		public Vector3 position;
		public Quaternion rotation;

		public Translation(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
		}
	}
}