using System;

using UnityEngine;


namespace ClockBlockers.ReplaySystem
{
	[Serializable]
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