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

        public GameObject casingPrefab;
        public GameObject muzzleFlashPrefab;
        public Transform barrelLocation;
        public Transform casingExitLocation;

        private AudioSource _audioSource;

        private Animator _animator;

        public float range;
        public float damage;
        public Enums.DamageType damageType;
        public bool canShoot;

        public BaseController holder;
        
        private static readonly int Shoot1 = Animator.StringToHash("Shoot");


        void Start()
        {
            if (barrelLocation == null)
            {
                barrelLocation = transform;
            }

            _animator = GetComponent<Animator>();

            _audioSource = GetComponent<AudioSource>();

            canShoot = true;
        }
        internal void PullTrigger()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _animator.SetTrigger(Shoot1);
            }
        }

        internal void Shoot()
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
            casing.GetComponent<Rigidbody>().AddExplosionForce(550f, position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f, 1f);
            casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);

            Destroy(casing, 1f);
        }

        private DamagePacket CreateDamagePacket()
        {
            return new DamagePacket(damageType, damage, this.gameObject);
        }

        private static void CreateBulletHole(Vector3 position, Quaternion rotation, Transform parentObject)
        {
            GameObject bulletHolePrefab = GameController.instance.bulletHoles[0];

            Vector3 bulletHoleScale = bulletHolePrefab.transform.localScale;
            Vector3 parentObjectScale = parentObject.lossyScale;


            var correctedScale = new Vector3(bulletHoleScale.x / parentObjectScale.x, bulletHoleScale.y / parentObjectScale.y, bulletHoleScale.z / parentObjectScale.z);
            GameObject bulletHole = Instantiate(bulletHolePrefab, position, rotation, parentObject);
            bulletHole.transform.localScale = correctedScale;

            Destroy(bulletHole, 5);
        }

        private void ShootRayCast()
        {
            Transform transform1 = holder.camera.transform;
            bool didHit = Physics.Raycast(transform1.position, transform1.forward, out var hit, range);

            if (!didHit) return;

            hit.transform.GetComponent<IInteractable>().OnHit(CreateDamagePacket(), hit.point);

            CreateBulletHole(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);

        }

    }
}
