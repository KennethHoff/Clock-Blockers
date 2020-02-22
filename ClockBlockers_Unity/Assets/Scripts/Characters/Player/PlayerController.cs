using System;
using System.Collections;
using System.Collections.Generic;
using DataStructures;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

public class PlayerController : BaseController
{

    [Header("Setup Variables")]


    //private List<String> currentFrameActions;

    private List<Tuple<Actions, string>> currentFrameActions;

    private Vector2 movementInput;

    private float upDowncameraRotation;
    private float sideToSideCharacterRotation;


    [Header("Player Setting")]
    public float verticalMouseSensitivity;
    public float horizontalMouseSensitivity;



    protected override void Awake()
    {
        base.Awake();
        currentFrameActions = new List<Tuple<Actions, string>>();
        characterActions = new List<CharacterAction>();
    }

    protected override void FixedUpdate()
    {
        MoveCharacterByInput();

        SaveCharacterActions();

        base.FixedUpdate();

    }

    private void Update()
    {
        RotateCharacter(sideToSideCharacterRotation);
        RotateCamera(upDowncameraRotation);
    }

    private void OnLook(InputValue ctx)
    {
        var value = ctx.Get<Vector2>();

        sideToSideCharacterRotation = value.x * horizontalMouseSensitivity * Time.deltaTime;
        upDowncameraRotation = value.y * verticalMouseSensitivity * Time.deltaTime;
    }

    private void OnMovement(InputValue ctx)
    {
        movementInput = ctx.Get<Vector2>();
    }

    private void OnSpawn()
    {
        StartCoroutine(WaitSpawnClone());
    }

    private void OnJump()
    {
        StartCoroutine(WaitAttemptToJump());
    }

    private void OnShoot(InputValue ctx)
    {
        if (ctx.isPressed) StartCoroutine(WaitAttemptToShoot());
    }

    private void OnClearClones()
    {
        Debug.Log("Clearing children");
        for (int i = 0; i < GameController.instance.cloneParent.childCount; i++)
        {
            Destroy(GameController.instance.cloneParent.GetChild(i).gameObject);
        }
    }

    private void OnIncreaseTimescale()
    {
        Time.timeScale += 1;
        Debug.Log("Increasing timescale. Now at: " + Time.timeScale);
    }

    private void OnDecreaseTimescale()
    {
        Time.timeScale -= 1;
        Debug.Log("Decreasing timescale. Now at: " + Time.timeScale);
    }
    private void SaveCharacterActions()
    {
        if (!recordActions) return;
        foreach (var action in currentFrameActions)
        {
            var newAction = new CharacterAction
            {
                action = action.Item1,
                parameter = action.Item2, 
                time = Time.fixedTime - spawnTime,
            };
            characterActions.Add(newAction);
        }
        currentFrameActions.Clear();
    }

    private void SaveActionAsString(Actions action, string parameters)
    {
        currentFrameActions.Add(Tuple.Create(action, parameters));

        if (debugLogEveryAction) Debug.Log("Time: " + Time.time + ". Function: " + action + ". Parameters:" + parameters);
    }

    private void SaveActionAsString(Actions action)
    {
        SaveActionAsString(action, "");
    }

    protected override void RotateCharacter(float rotation)
    {
        var stringedFloat = rotation.ToString(GameController.instance.FloatPointPrecisionString);
        var roundedFloat = float.Parse(stringedFloat);

        SaveActionAsString(Actions.RotateCharacter, stringedFloat);
        base.RotateCharacter(roundedFloat);
    }

    protected override void RotateCamera(float rotation)
    {
        var stringedFloat = rotation.ToString(GameController.instance.FloatPointPrecisionString);
        var roundedFloat = float.Parse(stringedFloat);

        SaveActionAsString(Actions.RotateCamera, stringedFloat);
        base.RotateCamera(roundedFloat);

    }


    private void MoveCharacterByInput()
    {
        if (movementInput.magnitude < 0.1f) return; // If no input, magnitude = 0. I don't want it to record every frame for all eternity. Only when moving.

        var timeAdjustedInput = movementInput * Time.fixedDeltaTime;
        MoveCharacterForward(new Vector3(timeAdjustedInput.x, 0, timeAdjustedInput.y));
    }

    protected override void MoveCharacterForward(Vector3 vector)
    {
        var roundedVector = vector.Round(GameController.instance.floatingPointPrecision);
        var stringedVector = roundedVector.ToString(GameController.instance.FloatPointPrecisionString);

        SaveActionAsString(Actions.Move, stringedVector);
        base.MoveCharacterForward(roundedVector);
    }

    protected override void AttemptToJump()
    {
        SaveActionAsString(Actions.Jump);
        base.AttemptToJump();
    }

    protected override void AttemptToShoot()
    {
        SaveActionAsString(Actions.Shoot);
        base.AttemptToShoot();
    }

    protected override void SpawnClone()
    {
        if (enableRecursiveClones) SaveActionAsString(Actions.SpawnClone);

        // TODO: BAD PRACTICE ALERT!
        SaveCharacterActions();

        //for (int i = 0; i < 100; i++)
        //{
        base.SpawnClone();
        //}
    }



    private IEnumerator WaitAttemptToJump()
    {
        yield return new WaitForFixedUpdate();
        AttemptToJump();
    }

    private IEnumerator WaitAttemptToShoot()
    {
        yield return new WaitForFixedUpdate();
        AttemptToShoot();
    }

    private IEnumerator WaitSpawnClone()
    {
        yield return new WaitForFixedUpdate();
        SpawnClone();
    }

}
