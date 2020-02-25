using UnityEngine;


namespace ClockBlockers.DataStructures
{
	public struct DamagePacket
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