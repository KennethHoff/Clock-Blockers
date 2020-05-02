using System;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.DataStructures;

using TMPro;

using UnityEngine;


namespace ClockBlockers.ToBeMoved {
	internal class HealthComponent : MonoBehaviour
	{
		private float Health
		{
			get => health;
			set => health.Value = Mathf.Clamp(value, 0, MaxHealth);
		}

		private Character _character;


		private float Armor => armor.Value;

		// private float Shielding
		// {
		// 	get => shielding.Value;
		// 	set => shielding.Value = value;
		// }


		private float MaxHealth => maxHealth.Value;
			
		[SerializeField]
		private FloatReference armor = null;

		// [SerializeField]
		// private FloatReference shielding;

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


		internal void DealDamage(DamagePacket damagePacket)
		{
			float finalDamage = damagePacket.damage - Armor;
			float remainingDamage = finalDamage;
			if (remainingDamage <= 0) return;

			// if (Shielding > 0)
			// {
			// 	if (Shielding >= remainingDamage)
			// 	{
			// 		Shielding -= remainingDamage;
			// 	}
			// 	else
			// 	{
			// 		remainingDamage -= Shielding;
			// 		Shielding = 0;
			// 	}
			// }

			Health -= remainingDamage;
			if (Health <= 0)
			{
				AttemptKill();
			}
		}
		private void AttemptKill()
		{
			_character.Kill();
		}
		
	}
}