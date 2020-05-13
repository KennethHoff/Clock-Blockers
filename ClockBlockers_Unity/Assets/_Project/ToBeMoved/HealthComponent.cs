using System;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.ToBeMoved.DataStructures;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.ToBeMoved {
	[BurstCompile]
	public class HealthComponent : MonoBehaviour
	{
		public float Health
		{
			get => health;
			set => health.Value = Mathf.Clamp(value, 0, MaxHealth);
		}

		private Character _character;

		private float Armor => armor.Value;

		private float MaxHealth => maxHealth.Value;
		public bool Dead { get; private set; }

		[SerializeField]
		private FloatReference armor = null;

		[SerializeField]
		private FloatReference maxHealth = null;

		[SerializeField]
		private FloatReference health = null;

		private void Awake()
		{
			_character = GetComponent<Character>();
		}

		private void Start()
		{
			health = maxHealth;
		}

		public void DealDamage(DamagePacket damagePacket)
		{
			float finalDamage = damagePacket.damage - Armor;
			float remainingDamage = finalDamage;
			if (remainingDamage <= 0) return;

			Health -= remainingDamage;

			if (Health <= 0)
			{
				Dead = true;
			}
		}
	}
}