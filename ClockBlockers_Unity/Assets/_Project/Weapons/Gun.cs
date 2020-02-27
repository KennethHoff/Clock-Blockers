using System;

using ClockBlockers.Characters.Scripts;
using ClockBlockers.DataStructures;

using JetBrains.Annotations;

using UnityEngine;

using Random = UnityEngine.Random;


namespace ClockBlockers.Weapons
{
	public class Gun : MonoBehaviour
	{
		
		
		[SerializeField]
		private GameObject[] bulletHoles;
		
		private AudioSource _audioSource;

		private Animator _animator;

		private float Damage
		{
			get => damage.Value;
		}

		public Character Holder
		{
			get => holder;
			set => holder = value;
		}

		[SerializeField]
		private GameObject casingPrefab;

		[SerializeField]
		private GameObject muzzleFlashPrefab;

		[SerializeField]
		private Transform barrelLocation;

		[SerializeField]
		private Transform casingExitLocation;

		[SerializeField]
		private Character holder;


		private float Range
		{
			get => range.Value;
		}

		[SerializeField]
		private FloatReference damage;

		[SerializeField]
		private FloatReference range;

		public bool CanShoot { get; private set; }

		private static int AnimationShootTrigger { get; } = Animator.StringToHash("Shoot");


		private void Start()
		{
			_animator = GetComponent<Animator>();

			_audioSource = GetComponent<AudioSource>();

			CanShoot = true;
		}

		internal void PullTrigger()
		{
			if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				_animator.SetTrigger(AnimationShootTrigger);
			}
		}

		[UsedImplicitly]
		private void Shoot()
		{
			GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

			Destroy(tempFlash, 0.5f);
			_audioSource.Play();
			ShootRayCast();
		}

		[UsedImplicitly]
		private void ReleaseCasing()
		{
			Vector3 position = casingExitLocation.position;
			GameObject casing = Instantiate(casingPrefab, position, casingExitLocation.rotation);
			casing.GetComponent<Rigidbody>().AddExplosionForce(550f,
				position - (casingExitLocation.right * 0.3f) - (casingExitLocation.up * 0.6f), 1f);
			casing.GetComponent<Rigidbody>()
				.AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);

			Destroy(casing, 1f);
		}

		private DamagePacket CreateDamagePacket()
		{
			return new DamagePacket(Damage, gameObject);
		}

		// ReSharper disable once UnusedParameter.Local
		private void CreateBulletHole(Vector3 position, Quaternion rotation, Transform target)
		{
			const float bulletHoleLifeTime = 5f;

			GameObject bulletHolePrefab = bulletHoles[0];

			GameObject bulletHole = Instantiate(bulletHolePrefab, position, rotation);

			Destroy(bulletHole, bulletHoleLifeTime);

			// bulletHole.transform.SetParent(target, true);
		}

		private void ShootRayCast()
		{
			Tuple<IInteractable, RaycastHit> target = holder.GetTarget(Range);

			if (target == null) return;

			IInteractable interactable = target.Item1;
			RaycastHit hit = target.Item2;

			interactable.OnHit(CreateDamagePacket(), hit.point);

			CreateBulletHole(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);
		}
	}
}