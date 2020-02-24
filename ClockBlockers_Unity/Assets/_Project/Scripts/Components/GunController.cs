using ClockBlockers.Characters;
using ClockBlockers.DataStructures;
using ClockBlockers.GameControllers;

using JetBrains.Annotations;

using UnityEngine;

using Random = UnityEngine.Random;



namespace ClockBlockers.Components
{
	public class GunController : MonoBehaviour
	{
		private GameObject CasingPrefab
		{
			get => casingPrefab;
		}

		private GameObject MuzzleFlashPrefab
		{
			get => muzzleFlashPrefab;
		}

		private Transform BarrelLocation
		{
			get => barrelLocation;
			set => barrelLocation = value;
		}

		private Transform CasingExitLocation
		{
			get => casingExitLocation;
		}

		private AudioSource Source { get; set; }

		private Animator Animator1 { get; set; }

		private float Range
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

		private bool CanShoot
		{
			get => canShoot;
			set => canShoot = value;
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

		[SerializeField]
		private bool canShoot;

		private static int AnimationShootTrigger { get; } = Animator.StringToHash("Shoot");


		private void Start()
		{
			if (BarrelLocation == null)
			{
				BarrelLocation = transform;
			}

			Animator1 = GetComponent<Animator>();

			Source = GetComponent<AudioSource>();

			CanShoot = true;
		}

		internal void PullTrigger()
		{
			if (Animator1.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				Animator1.SetTrigger(AnimationShootTrigger);
			}
		}

		[UsedImplicitly]
		private void Shoot()
		{
			GameObject tempFlash = Instantiate(MuzzleFlashPrefab, BarrelLocation.position, BarrelLocation.rotation);

			Destroy(tempFlash, 0.5f);
			Source.Play();
			ShootRayCast();
		}

		[UsedImplicitly]
		private void ReleaseCasing()
		{
			Vector3 position = CasingExitLocation.position;
			GameObject casing = Instantiate(CasingPrefab, position, CasingExitLocation.rotation);
			casing.GetComponent<Rigidbody>().AddExplosionForce(550f,
				position - CasingExitLocation.right * 0.3f - CasingExitLocation.up * 0.6f, 1f);
			casing.GetComponent<Rigidbody>()
				.AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);

			Destroy(casing, 1f);
		}

		private DamagePacket CreateDamagePacket()
		{
			return new DamagePacket(DamageType, Damage, this.gameObject);
		}

		private static void CreateBulletHole(Vector3 position, Quaternion rotation, Transform parentObject)
		{
			GameObject bulletHolePrefab = GameController.Instance.BulletHoles[0];

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
			Transform transform1 = Holder.Cam.transform;
			bool didHit = Physics.Raycast(transform1.position, transform1.forward, out var hit, Range);

			if (!didHit) return;

			hit.transform.GetComponent<IInteractable>().OnHit(CreateDamagePacket(), hit.point);

			CreateBulletHole(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);
		}
	}
}