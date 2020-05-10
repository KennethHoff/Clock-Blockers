using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.DataStructures
{
	[BurstCompile]
	public readonly struct DamagePacket
	{
		public readonly float damage;
		public readonly GameObject source;
		
		public DamagePacket(float damage, GameObject source)
		{
			this.damage = damage;
			this.source = source;
		}

	}
}