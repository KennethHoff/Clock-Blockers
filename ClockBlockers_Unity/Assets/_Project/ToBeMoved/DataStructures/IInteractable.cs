using UnityEngine;


namespace ClockBlockers.ToBeMoved.DataStructures
{
	public interface IInteractable
	{
		void OnHit(DamagePacket damagePacket, Vector3 hitPosition);
	}
}