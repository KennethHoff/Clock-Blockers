using DataStructures;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{

    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public Transform barrelLocation;
    public Transform casingExitLocation;

    private AudioSource audioSource;

    private Animator animator;

    public float range;
    public float damage;
    public Enums.DamageType damageType;
    public bool canShoot;

    public BaseController holder;


    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        canShoot = true;
    }
    internal void PullTrigger()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetTrigger("Shoot");
        }
    }

    internal void Shoot()
    {
        var tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

        Destroy(tempFlash, 0.5f);
        audioSource.Play();
        ShootRayCast();
    }

    void ReleaseCasing()
    {
        var casing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        casing.GetComponent<Rigidbody>().AddExplosionForce(550f, (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);

        Destroy(casing, 1f);
    }
    internal DamagePacket CreateDamagePacket()
    {
        return new DamagePacket(damageType, damage, this.gameObject);
    }

    internal void CreateBulletHole(Vector3 Position, Quaternion rotation, Transform parentObject)
    {
        var bulletHolePrefab = GameController.instance.bulletHoles[0];

        var bulletHoleScale = bulletHolePrefab.transform.localScale;
        var parentObjectScale = parentObject.lossyScale;


        Vector3 correctedScale = new Vector3(bulletHoleScale.x / parentObjectScale.x, bulletHoleScale.y / parentObjectScale.y, bulletHoleScale.z / parentObjectScale.z);
        var bulletHole = Instantiate(bulletHolePrefab, Position, rotation, parentObject);
        bulletHole.transform.localScale = correctedScale;

        //Destroy(bulletHole, 5);
    }

    protected void ShootRayCast()
    {
        var didHit = Physics.Raycast(holder.camera.transform.position, holder.camera.transform.forward, out var hit, range);

        if (!didHit)
            return;


        //Debug.Log(hit.point);

        hit.transform.GetComponent<IInteractable>().OnHit(CreateDamagePacket(), hit.point);

        CreateBulletHole(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);

        //var bulletHole = Instantiate(GameController.instance.bulletHoles[0], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform);
        //Destroy(bulletHole, 5);
    }

    //protected void ShootRayCast()
    //{
    //    var didHit = Physics.Raycast(holder.cam.transform.position, holder.cam.transform.forward, out var hit, range);

    //    if (didHit)
    //    {
    //        var baseController = hit.transform.GetComponent<BaseController>();
    //        if (baseController != null)
    //        {
    //            var damagePacket = new DamagePacket(damageType, damage, this.gameObject);
    //            baseController.AttemptDealDamage(damagePacket);
    //        }

    //        //Debug.Log( GetInstanceID() + " hit: " + hit.transform.name);
    //    }
    //    //else
    //    //{
    //    //    Debug.Log("GetInstanceID() + hit nothing!");
    //    //}



    //    return;


    //    if (hit.transform.gameObject.layer == GameController.instance.terrainLayerID)
    //    {
    //        // TODO: Add multiple Bullet Hole Textures
    //        var bulletHole = Instantiate(GameController.instance.bulletHoles[0], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
    //        Destroy(bulletHole, 5);
    //    }
    //    else
    //    if (hit.transform.gameObject.layer == GameController.instance.targetLayerID)
    //    {
    //        var point = hit.point;
    //        var hitPosition = hit.transform.position;
    //        var xDiff = point.x - hitPosition.x;
    //        var yDiff = point.y - hitPosition.y;
    //        var zDiff = point.z - hitPosition.z;
    //        Debug.Log("x Difference: " + xDiff.ToString("F10") + ". y Difference: " + yDiff.ToString("F10") + ". Z difference: " + zDiff.ToString("F10") + " | " + holder.name);
    //    }
    //}
}
