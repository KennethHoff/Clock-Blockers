using ClockBlockers.DataStructures;

using UnityEngine;


namespace ClockBlockers.ToBeMoved {
	internal class HealthComponent : MonoBehaviour
	{
		private float Health
		{
			get => health;
			set => health.Value = Mathf.Clamp(value, 0, MaxHealth);
		}


		private float Armor
		{
			get => armor.Value;
		}

		private float Shielding
		{
			get => shielding.Value;
			set => shielding.Value = value;
		}


		private float MaxHealth
		{
			get => maxHealth.Value;
		}

		[SerializeField]
		private FloatReference armor;

		[SerializeField]
		private FloatReference shielding;

		[SerializeField]
		private FloatReference maxHealth;

		[SerializeField]
		private FloatReference health;

		internal void DealDamage(DamagePacket damagePacket)
		{
			float finalDamage = damagePacket.damage - Armor;
			float remainingDamage = finalDamage;
			if (remainingDamage <= 0) return;

			if (Shielding > 0)
			{
				if (Shielding >= remainingDamage)
				{
					Shielding -= remainingDamage;
				}
				else
				{
					remainingDamage -= Shielding;
					Shielding = 0;
				}
			}

			Health -= remainingDamage;
			if (Health <= 0)
			{
				AttemptKill();
			}
		}

		private void AttemptKill()
		{
			Kill();
		}

		private void Kill()
		{
			// _bodyRenderer.material = gameController.DeadMaterial;
			//
			// const float removalTime = 1.25f;
			//
			// replayRunner.End();
			//
			// Destroy(gameObject, removalTime);
			//
			// StartCoroutine(Co_FallThroughFloor(removalTime));
		}
		

	}
}