using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Utility;

public abstract class BaseController : MonoBehaviour
{

    internal Camera cam;

    protected CharacterController characterController;

    [HideInInspector] public List<CharacterAction> characterActions; // Actions recorded
    [HideInInspector] public CharacterAction[] actionArray; // Actions received from "outside"

    /// <summary>
    /// the frame the character was spawned on. Used to know which frame it should perform which actions. (X frames after spawn .. )
    /// </summary>
    protected float spawnTime;

    protected float TimeAlive { get => Time.fixedTime - spawnTime; }

    protected Vector3 startPos;
    protected Quaternion startRot;

    [Header("Setup Variables")]
    public GameObject clonePrefab = null;
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
        spawnTime = Time.fixedTime;

        characterController = GetComponentInChildren<CharacterController>();

        startPos = transform.position;
        startRot = transform.rotation;

        cam = GetComponentInChildren<Camera>();

        gun.holder = this;
    }

    protected virtual void FixedUpdate()
    {
        AffectGravity();
        characterController.Move(moveVector);
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
        var currentAngle = cam.transform.rotation.eulerAngles;

        var newX = currentAngle.x - rotation;

        var preclampedX = newX > 180 ? newX - 360 : newX;
        var clampedX = Mathf.Clamp(preclampedX, minCamAngle, maxCamAngle);

        var newAngle = new Vector3(clampedX, 0, 0);

        cam.transform.localRotation = Quaternion.Euler(newAngle);
    }

    protected virtual void MoveCharacterForward(float x, float y)
    {
        var forward = transform.forward;
        var right = transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        var prelimMove = forward * y + right * x;
        var moveVector = prelimMove * moveSpd * Time.fixedDeltaTime;
        var roundedVector = moveVector.Round(6); // Useful for perfectly replicating the value later, if stored.


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

    internal void AttemptDealDamage(float gunDamage, Enums.DamageType gunDamageType)
    {
        DealDamage(gunDamage, gunDamageType);
    }

    /// <summary>
    /// Deal Damage means the entity in question is dealing damage, not dealing damage to other entities.
    ///
    /// ie: This deals damage to itself.
    /// </summary>
    /// <param name="gunDamage"></param>
    /// <param name="gunDamageType"></param>
    private void DealDamage(float gunDamage, Enums.DamageType gunDamageType)
    {
        currHealth -= gunDamage;
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
        GetComponentInChildren<CharacterBodyController>().GetComponent<Renderer>().material = GameController.gc.deadMaterial;
        StopAllCoroutines();
        Destroy(this.gameObject, 1.25f);
    }


    protected virtual void SpawnClone()
    {

        var newClone = Instantiate(clonePrefab, startPos, startRot, GameController.gc.cloneParent);

        var newCloneController = newClone.GetComponent<CloneController>();

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
    }
}
