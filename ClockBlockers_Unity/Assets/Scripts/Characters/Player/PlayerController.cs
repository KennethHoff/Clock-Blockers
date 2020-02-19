using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using System.Reflection;
using System.Text.RegularExpressions;

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


    private float minRotateValue = 0.1f;


    protected override void Awake()
    {
        base.Awake();
        currentFrameActions = new List<Tuple<string, string>>();
        characterActions = new List<CharacterAction>();
    }

    protected override void FixedUpdate()
    {
        MoveCharacterByInput();
        RotateCharacter(sideToSideCharacterRotation);
        RotateCamera(upDowncameraRotation);

        SaveCharacterActions();

        base.FixedUpdate();

    }

    private void OnLook(InputValue ctx)
    {
        var value = ctx.Get<Vector2>();

        sideToSideCharacterRotation = value.x * horizontalMouseSensitivity * Time.fixedDeltaTime;
        upDowncameraRotation = value.y * verticalMouseSensitivity * Time.fixedDeltaTime;
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

    //private void SaveCharacterActions()
    //{
    //    if (!recordActions) return;
    //    foreach (var method in currentFrameActions)
    //    {
    //        var newAction = new CharacterAction { method = method, time = Time.fixedTime - spawnTime };
    //        characterActions.Add(newAction);
    //    }
    //    currentFrameActions.Clear();
    //}

    //private void SaveActionAsString(string functionName, string parameters)
    //{
    //    var funcName = "func: " + functionName;
    //    var para = "params: " + parameters;
    //    var finalString = funcName + para;

    //    currentFrameActions.Add(finalString);



    //    //var newAction = new CharacterAction() {method = finalString, time = Time.fixedTime - spawnTime};
    //    //characterActions.Add(newAction);
    //}

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
    }

    private void SaveActionAsString(string functionName)
    {
        SaveActionAsString(functionName, "");
    }

    protected override void RotateCharacter(float rotation)
    {
        if (Mathf.Abs(rotation) >= minRotateValue)
        {
            SaveActionAsString("rotateCharacter", rotation.ToString("F6"));
            base.RotateCharacter(rotation);

            // After rotating, set the current rotation to 0. I set the rotation in the "Mouse moved" event.
            // And then every frame rotate the camera based on that, but if I don't reset it it'll keep turning.
            sideToSideCharacterRotation = 0;
        }

    }

    protected override void RotateCamera(float rotation)
    {

        if (Mathf.Abs(rotation) >= minRotateValue)
        {
            SaveActionAsString("rotateCamera", rotation.ToString("F6"));
            base.RotateCamera(rotation);
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

    protected override void spawnClone()
    {
        if (enableRecursiveClones) SaveActionAsString("spawnClone");

        // TODO: BAD PRACTICE ALERT!
        SaveCharacterActions();

        //for (int i = 0; i < 100; i++)
        //{
        base.spawnClone();
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
        spawnClone();
    }

}
