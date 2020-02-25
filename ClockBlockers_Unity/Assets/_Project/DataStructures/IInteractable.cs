using UnityEngine;


namespace ClockBlockers.DataStructures
{
	public interface IInteractable
	{
		void OnHit(DamagePacket damagePacket, Vector3 hitPosition);
	}
}