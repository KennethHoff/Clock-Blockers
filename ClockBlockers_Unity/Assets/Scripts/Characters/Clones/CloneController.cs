using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Utility;

public class CloneController : BaseController
{
    private int currentActionIndex = 0;
    [Header("Setup Values")]
    public Material CompletedMaterial;

    public CharacterAction[] actionArray;

    public int RemainingActions { get => actionArray.Length - currentActionIndex; }

    public float TimeUntilLastActionIsCompleted
    {
        get => actionArray.Last().time - TimeAlive;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CheckIfTimeForAction();
    }

    private void CheckIfTimeForAction()
    {
        if (HaveCompletedAllActions()) return;
        var curTime = Time.time - spawnTime;

        RunAllActionsThisFrame(curTime);

    }

    private void RunAllActionsThisFrame(float curTime)
    {
        if (actionArray[currentActionIndex].time <= curTime)
        {
            //Debug.Log("Checking time of index: " + currentActionIndex);
            RetrieveActionFromString(actionArray[currentActionIndex].action);
            currentActionIndex++;

            if (HaveCompletedAllActions()) return;
            RunAllActionsThisFrame(curTime);
        }
    }

    private bool HaveCompletedAllActions()
    {
        var value = actionArray.Length <= currentActionIndex;

        if (value)
        {
            GetComponentInChildren<Renderer>().material = CompletedMaterial;
            return true;
        }

        return false;
    }
    private void RetrieveActionFromString(string actionString)
    {
        var funcName = Regex.Match(actionString, "(?<=func: ).*?(?=params: )").ToString(); // Get function, by string.
        var para = Regex.Match(actionString, "(?<=params: ).*").ToString(); // Get parameters, by string.


        switch (funcName)
        {
            case "moveCharacter":
                var moveVector = UsefulMethods.StringToVector3(para);
                MoveCharacterViaAction(moveVector);
                break;
            case "rotateCharacter":
                var xRotation = float.Parse(para);
                RotateCharacterViaAction(xRotation);
                break;
            case "jumpCharacter":
                AttemptToJump();
                break;
            default:
                Debug.Log(funcName + " is not a valid funcName");
                break;
        }
    }

    public void RotateCharacterViaAction(float rotation)
    {
        //Debug.Log("Rotating via Action.");
        RotateCharacter(rotation);
    }

    public void MoveCharacterViaAction(Vector3 moveVector)
    {
        //Debug.Log("Moving via Action");
        MoveCharacterForward(moveVector.x, moveVector.z);
    }

}
