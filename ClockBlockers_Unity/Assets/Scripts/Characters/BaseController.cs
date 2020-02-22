using System;
using System.Collections;
using System.Collections.Generic;
using DataStructures;
using UnityEngine;
using Utility;

public abstract class BaseController : MonoBehaviour, IInteractable
{

    internal new Camera camera;

    protected CharacterController characterController;

    [HideInInspector] public List<CharacterAction> characterActions; // Actions recorded
    [HideInInspector] public CharacterAction[] actionArray; // Actions received from "outside"

    /// <summary>
    /// the frame the character was spawned on. Used to know which frame it should perform which actions. (X frames after spawn .. )
    /// </summary>
    protected float spawnTime;

    protected float TimeAlive { get => Time.fixedTime - spawnTime; }

    /// <summary>
    /// World position
    /// </summary>
    protected Vector3 startPos;

    /// <summary>
    /// World rotation
    /// </summary>
    protected Quaternion startRot;

    /// <summary>
    /// Local position, relative to the parent object of the character.
    /// </summary>
    protected Vector3 camStartPos;
    /// <summary>
    /// Local rotation, relative to the parent object of the character.
    /// </summary>
    protected Quaternion camStartRot;

    [Header("Setup Variables")]
    public GameObject clonePrefab;
    public GunController gun;

    [Header("Debug Variables")] 
    public bool debugLogEveryAction;

    [Header("Character Variables")]
    public float moveSpd;
    public float jumpVelocity;

    public float minCamAngle;
    public float maxCamAngle;

    public bool recordActions = false;
    
    public bool enableRecursiveClones;
    [Space(10)]
    public float maxHealth;
    public float currHealth;
    public float armor;
    public float shielding;
    public bool isAlive;


    protected Vector3 moveVector;


    protected virtual void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        characterController = GetComponentInChildren<CharacterController>();
        gun.holder = this;

    }

    protected virtual void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        camStartPos = camera.transform.localPosition;
        camStartRot = camera.transform.localRotation;

        spawnTime = Time.fixedTime;

    }

    protected virtual void FixedUpdate()
    {
        //AffectGravity();
        //characterController.Move(moveVector);
    }

    protected void AffectGravity()
    {
        //if (characterController.velocity.y == 0) moveVector.y = 0;
        if (characterController.isGrounded)
        {
            var tempGrav = Physics.gravity * 0.1f;
            if (moveVector.y < tempGrav.y) moveVector = tempGrav;
            return;
        }

        moveVector += Physics.gravity * Time.fixedDeltaTime;
    }


    protected virtual void AttemptToJump()
    {

        if (characterController.isGrounded)
        {
            ExecuteJump();
        }
        
    }
    protected virtual void ExecuteJump()
    {
        moveVector = Vector3.up * jumpVelocity;
        //Debug.Log("Jumped!");
    }

    protected virtual void RotateCharacter(float yRotation)
    {
        var newAngle = new Vector3(0, yRotation, 0);
        transform.Rotate(newAngle);
    }

    protected virtual void RotateCamera(float rotation)
    {
        var currentAngle = camera.transform.rotation.eulerAngles;

        var newX = currentAngle.x - rotation;

        var preclampedX = newX > 180 ? newX - 360 : newX;
        var clampedX = Mathf.Clamp(preclampedX, minCamAngle, maxCamAngle);

        var newAngle = new Vector3(clampedX, 0, 0);

        camera.transform.localRotation = Quaternion.Euler(newAngle);
    }

    protected virtual void MoveCharacterForward(Vector3 vector)
    {
        var forward = transform.forward;
        var right = transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        var prelimMove = forward * vector.z + right * vector.x;
        var moveVector = prelimMove * moveSpd;
        var roundedVector = moveVector.Round(GameController.instance.floatingPointPrecision); // Useful for perfectly replicating the value later, if stored.


        //Debug.Log("Clone Moving in the following vector: " + roundedVector.ToString("F6"));
        MoveCharacter(roundedVector);
    }

    protected void MoveCharacter(Vector3 vector)
    {
        characterController.Move(vector);
    }

    protected virtual void AttemptToShoot()
    {

        // TODO: Add Ammo checks etc..
        gun.PullTrigger();
    }


    protected void HealToFull()
    {
        currHealth = maxHealth;
    }

    internal void AttemptDealDamage(DamagePacket damagePacket)
    {
        DealDamage(damagePacket);
    }

    /// <summary>
    /// Deal Damage means the entity in question is dealing damage, not dealing damage to other entities.
    ///
    /// ie: This deals damage to itself.
    /// </summary>
    /// <param name="gunDamage"></param>
    /// <param name="gunDamageType"></param>
    private void DealDamage(DamagePacket damagePacket)
    {
        currHealth -= damagePacket.damage;
        if (currHealth <= 0)
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
        GetComponentInChildren<CharacterBodyController>().GetComponent<Renderer>().material = GameController.instance.deadMaterial;
        StopAllCoroutines();
        Destroy(this.gameObject, 1.25f);
    }


    protected virtual void SpawnClone()
    {
        var newClone = Instantiate(clonePrefab, startPos, startRot, GameController.instance.cloneParent);

        var newCloneController = newClone.GetComponent<CloneController>();

        newCloneController.camera.transform.localRotation = camStartRot;

        //newCloneController.cam.transform.position = camStartPos;
        //newCloneController.cam.transform.rotation = camStartRot;

        if (GetComponent<PlayerController>()) newCloneController.actionArray = characterActions.ToArray();
        else if (GetComponent<CloneController>()) newCloneController.actionArray = actionArray;
        else
        {
            Debug.LogError("Somewhat terrible has happened - How did a non-clone, non-player create a clone?");
            Debug.Break();
        }
        
        newCloneController.moveSpd = moveSpd;
        newCloneController.jumpVelocity = jumpVelocity;

        newCloneController.maxHealth = maxHealth;

        Debug.Log("Position: " + transform.position.ToString("F10") + " | Rotation: " + transform.rotation.ToString("F10") + " | Name: " + name);

    }

    public void OnHit(DamagePacket damagePacket, Vector3 hitPosition)
    {
        AttemptDealDamage(damagePacket);
    }
}
