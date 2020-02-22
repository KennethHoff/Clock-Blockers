using UnityEngine;

namespace DataStructures
{
    public interface IInteractable
    {
        void OnHit(DamagePacket damagePacket, Vector3 hitPosition);
    }
}