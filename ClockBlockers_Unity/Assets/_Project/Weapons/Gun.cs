using System;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.DataStructures;
using ClockBlockers.Targetting;

using JetBrains.Annotations;

using UnityEngine;

using Random = UnityEngine.Random;


namespace ClockBlockers.Weapons
{
	public class Gun : MonoBehaviour
	{
		private IRayProvider _rayProvider;

		private ITargeter _targeter;
		
		
		[SerializeField]
		private GameObject[] bulletHoles = null;
		
		private AudioSource _audioSource;

		private Animator _animator;

		private float Damage => damage.Value;

		// public Character Holder
		// {
		// 	get => holder;
		// 	set => holder = value;
		// }

		[SerializeField]
		private GameObject casingPrefab = null;

		[SerializeField]
		private GameObject muzzleFlashPrefab = null;

		[SerializeField]
		private Transform barrelLocation = null;

		[SerializeField]
		private Transform casingExitLocation = null;

		[SerializeField]
		private Character holder;
		
		[SerializeField]
		private FloatReference damage = null;

		[SerializeField]
		private FloatReference range = null;

		// public bool CanShoot { get; private set; }

		private static int AnimationShootTrigger { get; } = Animator.StringToHash("Shoot");


		private void Awake()
		{
			_rayProvider = GetComponent<IRayProvider>();
			_targeter = GetComponent<ITargeter>();
		}

		private void Start()
		{
			_animator = GetComponent<Animator>();

			_audioSource = GetComponent<AudioSource>();

			// CanShoot = true;
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
			Tuple<IInteractable, RaycastHit> target = GetTarget();

			if (target == null) return;

			IInteractable interactable = target.Item1;
			RaycastHit hit = target.Item2;

			interactable.OnHit(CreateDamagePacket(), hit.point);

			CreateBulletHole(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);
		}

		private Ray CreateRay()
		{
			return _rayProvider.CreateRay();
		}

		private Tuple<IInteractable, RaycastHit> GetTarget()
		{
			return _targeter.GetInteractableFromRay(CreateRay(), range);
		}
	}
}