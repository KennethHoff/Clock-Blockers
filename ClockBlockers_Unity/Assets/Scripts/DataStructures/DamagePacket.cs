using UnityEngine;

namespace ClockBlockers.DataStructures
{
    public struct DamagePacket
    {
        public DamagePacket(Enums.DamageType damageType, float damage, GameObject source)
        {
            this.damageType = damageType;
            this.damage = damage;
            this.source = source;
        }

        public Enums.DamageType damageType;
        public float damage;
        public GameObject source;
    }
}