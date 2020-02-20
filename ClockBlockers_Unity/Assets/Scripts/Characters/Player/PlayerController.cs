using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

public class PlayerController : BaseController
{

    [Header("Setup Variables")]


    //private List<String> currentFrameActions;

    private List<Tuple<string, string>> currentFrameActions;

    private Vector2 movementInput;

    private float upDowncameraRotation;
    private float sideToSideCharacterRotation;


    [Header("Player Setting")]
    public float verticalMouseSensitivity;
    public float horizontalMouseSensitivity;


    private readonly float minRotateValue = 0.0001f;


    protected override void Awake()
    {
        base.Awake();
        currentFrameActions = new List<Tuple<string, string>>();
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
        Debug.Log("Clearing childrens");
        for (int i = 0; i < GameController.gc.cloneParent.childCount; i++)
        {
            Destroy(GameController.gc.cloneParent.GetChild(i).gameObject);
        }
    }

    private void OnIncreaseTimescale()
    {
        Debug.Log("Increasing timescale. Now at: " + Time.timeScale);
        Time.timeScale += 1;
    }

    private void OnDecreaseTimescale()
    {
        Debug.Log("Decreasing timescale. Now at: " + Time.timeScale);
        Time.timeScale -= 1;
    }
    private void SaveCharacterActions()
    {
        if (!recordActions) return;
        foreach (var action in currentFrameActions)
        {
            var newAction = new CharacterAction
            {
                method = action.Item1,
                parameter = action.Item2, 
                time = Time.fixedTime - spawnTime
            };
            characterActions.Add(newAction);
        }
        currentFrameActions.Clear();
    }

    private void SaveActionAsString(string functionName, string parameters)
    {
        currentFrameActions.Add(Tuple.Create(functionName, parameters));

        if (debugLogEveryAction) Debug.Log("Time: " + Time.time + ". Function: " + functionName + ". Parameters:" + parameters);
    }

    private void SaveActionAsString(string functionName)
    {
        SaveActionAsString(functionName, "");
    }

    protected override void RotateCharacter(float rotation)
    {
        if (Mathf.Abs(rotation) >= minRotateValue)
        {
            var stringedFloat = rotation.ToString(GameController.gc.FloatPointPrecisionString);
            var roundedFloat = float.Parse(stringedFloat);

            SaveActionAsString("rotateCharacter", stringedFloat);
            base.RotateCharacter(roundedFloat);

            // After rotating, set the current rotation to 0. I set the rotation in the "Mouse moved" event.
            // And then every frame rotate the camera based on that, but if I don't reset it it'll keep turning.
            sideToSideCharacterRotation = 0;
        }

    }

    protected override void RotateCamera(float rotation)
    {

        if (Mathf.Abs(rotation) >= minRotateValue)
        {

            var stringedFloat = rotation.ToString(GameController.gc.FloatPointPrecisionString);
            var roundedFloat = float.Parse(stringedFloat);

            SaveActionAsString("rotateCamera", stringedFloat);
            base.RotateCamera(roundedFloat);

            // After rotating, set the current rotation to 0. I set the rotation in the "Mouse moved" event.
            // And then every frame rotate the camera based on that, but if I don't reset it it'll keep turning.
            upDowncameraRotation = 0;
        }

    }


    private void MoveCharacterByInput()
    {
        if (movementInput.magnitude < 0.1f) return;

        MoveCharacterForward(movementInput.x, movementInput.y);
    }

    protected override void MoveCharacterForward(float x, float y)
    {
        var cloneVector = new Vector3(x, 0, y).Round(6);

        SaveActionAsString("moveCharacter", cloneVector.ToString("F6"));
        base.MoveCharacterForward(x, y);
    }

    protected override void AttemptToJump()
    {
        SaveActionAsString("jumpCharacter");
        base.AttemptToJump();
    }

    protected override void AttemptToShoot()
    {
        SaveActionAsString("shootGun");
        base.AttemptToShoot();
    }

    protected override void SpawnClone()
    {
        if (enableRecursiveClones) SaveActionAsString("spawnClone");

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
