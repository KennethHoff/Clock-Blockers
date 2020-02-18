using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Utility;

public abstract class BaseController : MonoBehaviour
{
    protected CharacterController characterController;

    /// <summary>
    /// the frame the character was spawned on. Used to know which frame it should perform which actions. (X frames after spawn .. )
    /// </summary>
    protected float spawnTime;

    protected float TimeAlive { get => Time.time - spawnTime; }

    [Header("Setup Variables")]

    protected Vector3 startPos;
    protected Quaternion startRot;

    [Header("Character Values")]
    public float moveSpd;
    public float maxHealth;
    public float currHealth;

    public float jumpVelocity;

    public float fallMultipler = 2.5f;
    public float lowJumpMultiplier = 2f;

    public bool recordActions = false;
    

    /// <summary>
    /// If Space is being held, you jump higher
    /// </summary>
    protected bool holdingSpace;

    [Header("Debug Values")]
    [SerializeField] protected Vector3 moveVector;



    protected virtual void Awake()
    {
        spawnTime = Time.time;

        characterController = GetComponent<CharacterController>();

        startPos = transform.position;
        startRot = transform.rotation;
    }

    protected virtual void Update()
    {
        characterController.Move(moveVector);
    }

    protected void FixedUpdate()
    {
        AffectGravity();
    }

    protected void AffectGravity()
    {
        if (characterController.isGrounded) return;

        if (characterController.velocity.y <= 0)
        {
            //characterController.Move(Physics.gravity * (fallMultipler - 1) * Time.deltaTime);
            moveVector += Physics.gravity * (fallMultipler - 1) * Time.fixedDeltaTime;
        }
        else if (characterController.velocity.y > 0 && !holdingSpace)
        {
            //characterController.Move(Physics.gravity * (lowJumpMultiplier - 1) * Time.deltaTime);
            moveVector += Physics.gravity * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        moveVector += Physics.gravity * Time.fixedDeltaTime;
    }


    protected void AttemptToJump()
    {
        Debug.Log("Attempting to Jump!");
        if (characterController.isGrounded)
        {
            ExecuteJump();
        }
        else
        {
            Debug.Log("Can't jump. Not on ground!");
        }
    }

    protected virtual void ExecuteJump()
    {
        moveVector = Vector3.up * jumpVelocity;
        Debug.Log("Jumped!");
    }

    protected virtual void RotateCharacter(float yRotation)
    {
        var newAngle = new Vector3(0, yRotation, 0);
        transform.Rotate(newAngle);
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
        var moveVector = prelimMove * moveSpd * Time.deltaTime;
        var roundedVector = moveVector.Round(6); // Useful for perfectly replicating the value later, if stored.


        //Debug.Log("Clone Moving in the following vector: " + roundedVector.ToString("F6"));
        MoveCharacter(roundedVector);
    }

    protected void MoveCharacter(Vector3 vector)
    {
        characterController.Move(vector);
    }

}
