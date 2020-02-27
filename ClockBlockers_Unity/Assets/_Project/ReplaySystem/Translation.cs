using UnityEngine;


namespace ClockBlockers.ReplaySystem
{
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