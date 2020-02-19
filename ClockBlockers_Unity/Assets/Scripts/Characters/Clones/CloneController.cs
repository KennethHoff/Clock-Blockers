using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text.RegularExpressions;
using UnityEngine;
using Utility;

public class CloneController : BaseController
{
    private void Start()
    {
        EngageAllActions();
    }

    private void EngageAllActions()
    {
        foreach (var characterAction in actionArray)
        {
            RunActionFromString(characterAction);
        }
    }
    private void RunActionFromString(CharacterAction charAction)
    {

        //var funcName = Regex.Match(actionString, "(?<=func: ).*?(?=params: )").ToString(); // Get function, by string.
        //var para = Regex.Match(actionString, "(?<=params: ).*").ToString(); // Get parameters, by string.

        //switch (funcName)
        //{
        //    case "moveCharacter":
        //        var move = UsefulMethods.StringToVector3(para);
        //        StartCoroutine(WaitMoveCharacterViaAction(move, charAction.time));
        //        break;
        //    case "rotateCharacter":
        //        var charRot = float.Parse(para);
        //        StartCoroutine(WaitRotateCharacterViaAction(charRot, charAction.time));
        //        break;
        //    case "jumpCharacter":
        //        StartCoroutine(WaitJumpCharacterViaAction(charAction.time));
        //        break;
        //    case "rotateCamera":
        //        var camRot = float.Parse(para);
        //        StartCoroutine(WaitRotateCameraViaAction(camRot, charAction.time));
        //        break;
        //    case "shootGun":
        //        StartCoroutine(WaitShootGunViaAction(charAction.time));
        //        break;
        //    case "spawnClone":
        //        StartCoroutine(WaitSpawnClone(charAction.time));
        //        break;
        //    default:
        //        Debug.Log(funcName + " is not a valid funcName");
        //        break;
        //}

        var actionString = charAction.method;
        var paramString = charAction.parameter;

        switch (actionString)
        {
            case "moveCharacter":
                var move = UsefulMethods.StringToVector3(paramString);
                StartCoroutine(WaitMoveCharacterViaAction(move, charAction.time));
                break;
            case "rotateCharacter":
                var charRot = float.Parse(paramString);
                StartCoroutine(WaitRotateCharacterViaAction(charRot, charAction.time));
                break;
            case "jumpCharacter":
                StartCoroutine(WaitJumpCharacterViaAction(charAction.time));
                break;
            case "rotateCamera":
                var camRot = float.Parse(paramString);
                StartCoroutine(WaitRotateCameraViaAction(camRot, charAction.time));
                break;
            case "shootGun":
                StartCoroutine(WaitShootGunViaAction(charAction.time));
                break;
            case "spawnClone":
                StartCoroutine(WaitSpawnClone(charAction.time));
                break;
            default:
                Debug.Log(actionString + " is not a valid Method Name");
                break;
        }
    }

    private IEnumerator WaitSpawnClone(float timeToOccur)
    {
        yield return new WaitForSeconds(timeToOccur - Time.fixedDeltaTime);
        yield return new WaitForFixedUpdate();
        spawnClone();
    }

    private IEnumerator WaitShootGunViaAction(float timeToOccur)
    {
        yield return new WaitForSeconds(timeToOccur - Time.fixedDeltaTime);
        yield return new WaitForFixedUpdate();
        AttemptToShoot();
    }

    private IEnumerator WaitRotateCameraViaAction(float rotation, float timeToOccur)
    {
        yield return new WaitForSeconds(timeToOccur - Time.fixedDeltaTime);
        yield return new WaitForFixedUpdate();
        RotateCameraViaAction(rotation);
    }

    private void RotateCameraViaAction(float rotation)
    {
        RotateCamera(rotation);
    }

    public void RotateCharacterViaAction(float rotation)
    {
        RotateCharacter(rotation);
    }

    public void MoveCharacterViaAction(Vector3 move)
    {
        MoveCharacterForward(move.x, move.z);
    }


    private IEnumerator WaitMoveCharacterViaAction(Vector3 move, float timeToOccur)
    {
        yield return new WaitForSeconds(timeToOccur - Time.fixedDeltaTime);
        yield return new WaitForFixedUpdate();
        MoveCharacterViaAction(move);
    }

    private IEnumerator WaitRotateCharacterViaAction(float rotation, float timeToOccur)
    {
        yield return new WaitForSeconds(timeToOccur - Time.fixedDeltaTime);
        yield return new WaitForFixedUpdate();
        RotateCharacter(rotation);
    }

    private IEnumerator WaitJumpCharacterViaAction(float timeToOccur)
    {
        yield return new WaitForSeconds(timeToOccur - Time.fixedDeltaTime);
        yield return new WaitForFixedUpdate();
        AttemptToJump();
    }
}
