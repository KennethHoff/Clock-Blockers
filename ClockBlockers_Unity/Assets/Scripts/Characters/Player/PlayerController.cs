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
    private Camera cam;

    [Header("Setup Variables")]

    [SerializeField] private GameObject clonePrefab = null;

    [SerializeField] private Transform cloneParent = null;

    /// <summary>
    /// Actions executed this frame
    /// </summary>
    private List<String> currentFrameActions;

    public List<CharacterAction> characterActions;

    private Vector2 movementInput;

    private float cameraRotation;
    private float characterRotation;


    [Header("Player Setting")]
    public float verticalMouseSensitivity;
    public float horizontalMouseSensitivity;


    private float minRotateValue = 0.1f;


    protected override void Awake()
    {
        base.Awake();
        currentFrameActions = new List<string>();
        characterActions = new List<CharacterAction>();

        cam = GetComponentInChildren<Camera>();
    }

    protected override void FixedUpdate()
    {
        MoveCharacterByInput();
        RotateCharacter(characterRotation);
        RotateCamera(cameraRotation);

        SaveCharacterActions();

        base.FixedUpdate();

    }
    
    public void OnLook(InputValue ctx)
    {
        var value = ctx.Get<Vector2>();

        characterRotation = value.x * horizontalMouseSensitivity * Time.fixedDeltaTime;
        cameraRotation = value.y * verticalMouseSensitivity * Time.fixedDeltaTime;
    }

    public void OnMovement(InputValue ctx)
    {
        movementInput = ctx.Get<Vector2>();
    }

    public void OnSpawn()
    {
        CreateClone();
    }

    public void OnJump(InputValue ctx)
    {
        //holdingSpace = ctx.isPressed;
        //if (holdingSpace) AttemptToJump();
        AttemptToJump();
    }

    public void OnClearClones()
    {
        Debug.Log("Clearing children");
        for (int i = 0; i < cloneParent.childCount; i++)
        {
            Destroy(cloneParent.GetChild(i).gameObject);
        }
    }

    public void RotateCamera(float rotation)
    {

        var minAngle = -90;
        var maxAngle = 90;

        var currentAngle = cam.transform.rotation.eulerAngles;

        var newX = currentAngle.x - rotation;

        var preclampedX = newX > 180 ? newX - 360 : newX;
        var clampedX = Mathf.Clamp(preclampedX, minAngle, maxAngle);

        var newAngle = new Vector3(clampedX, 0, 0);

        cam.transform.localRotation = Quaternion.Euler(newAngle);
    }

    public void RotateCameraViaInput()
    {
        RotateCamera(cameraRotation);
        cameraRotation = 0;
    }

    private void SaveCharacterActions()
    {
        if (!recordActions) return;
        foreach (var action in currentFrameActions)
        {
            var newAction = new CharacterAction {action = action, time = Time.time - spawnTime};
            characterActions.Add(newAction);
        }
        currentFrameActions.Clear();
    }

    private void SaveActionAsString(string functionName, string parameters)
    {
        var funcName = "func: " + functionName;
        var para = "params: " + parameters;
        var finalString = funcName + para;
        currentFrameActions.Add(finalString);

        //Debug.Log("Saved: " + currentFrameActions.Last());
    }

    protected override void RotateCharacter(float yRotation)
    {
        if (Mathf.Abs(yRotation) >= minRotateValue)
        {
            base.RotateCharacter(yRotation);
        }

        SaveActionAsString("rotateCharacter", yRotation.ToString("F6"));

        // After rotating, set the current rotation to 0. I set the rotation in the "Mouse moved" event.
        // And then every frame rotate the camera based on that, but if I don't reset it it'll keep turning.
        cameraRotation = 0;
    }


    private void MoveCharacterByInput()
    {
        if (movementInput.magnitude < 0.1f) return;

        MoveCharacterForward(movementInput.x, movementInput.y);
    }

    protected override void MoveCharacterForward(float x, float y)
    {
        base.MoveCharacterForward(x, y);
        var cloneVector = new Vector3(x, 0, y).Round(6);

        SaveActionAsString("moveCharacter", cloneVector.ToString("F6"));
    }

    protected override void ExecuteJump()
    {
        base.ExecuteJump();
        SaveActionAsString("jumpCharacter", "");
    }


    public GameObject CreateClone()
    {

        var newClone = Instantiate(clonePrefab, startPos, startRot, cloneParent);

        var newCloneController = newClone.GetComponent<CloneController>();

        newCloneController.actionArray = characterActions.ToArray();

        newCloneController.moveSpd = moveSpd;
        newCloneController.jumpVelocity = jumpVelocity;
        newCloneController.fallMultipler = fallMultipler;
        newCloneController.lowJumpMultiplier = lowJumpMultiplier;

        return newClone;
    }
}
