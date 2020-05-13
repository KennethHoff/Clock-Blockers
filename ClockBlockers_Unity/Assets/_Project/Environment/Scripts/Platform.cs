using ClockBlockers.ToBeMoved.DataStructures;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.Environment
{
	[BurstCompile]

	public class Platform : MonoBehaviour, IInteractable
	{
		public void OnHit(DamagePacket damagePacket, Vector3 hitPosition) { }
	}
}