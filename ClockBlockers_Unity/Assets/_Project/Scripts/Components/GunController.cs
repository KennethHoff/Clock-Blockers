using System;

using ClockBlockers.Characters;
using ClockBlockers.DataStructures;
using ClockBlockers.GameControllers;
using ClockBlockers.Utility;

using JetBrains.Annotations;

using UnityEngine;

using Random = UnityEngine.Random;


namespace ClockBlockers.Components
{
	public class GunController : MonoBehaviour
	{
		private AudioSource _audioSource;

		private Animator _animator;

		public float Range
		{
			get => range;
		}

		private float Damage
		{
			get => damage;
		}

		private Enums.DamageType DamageType
		{
			get => damageType;
		}

		public BaseController Holder
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
		private BaseController holder;

		[SerializeField]
		private float range;

		[SerializeField]
		private float damage;

		[SerializeField]
		private Enums.DamageType damageType;

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
			return new DamagePacket(DamageType, Damage, gameObject);
		}

		private static void CreateBulletHole(Vector3 position, Quaternion rotation, Transform parentObject)
		{
			GameObject bulletHolePrefab = GameController.instance.BulletHoles[0];

			Vector3 bulletHoleScale = bulletHolePrefab.transform.localScale;
			Vector3 parentObjectScale = parentObject.lossyScale;


			var correctedScale = new Vector3(bulletHoleScale.x / parentObjectScale.x,
				bulletHoleScale.y / parentObjectScale.y, bulletHoleScale.z / parentObjectScale.z);
			GameObject bulletHole = Instantiate(bulletHolePrefab, position, rotation, parentObject);
			bulletHole.transform.localScale = correctedScale;

			Destroy(bulletHole, 5);
		}

		private void ShootRayCast()
		{
			Tuple<IInteractable, RaycastHit> target = holder.GetTarget();
			
			if (target == null) return;

			IInteractable interactable = target.Item1;
			RaycastHit hit = target.Item2;

			if (hit.distance > range) return;
			
			interactable.OnHit(CreateDamagePacket(), hit.point);

			CreateBulletHole(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);
		}
	}
}