using System.Collections;
using ClockBlockers.DataStructures;
using ClockBlockers.Utility;
using UnityEngine;

namespace ClockBlockers.Characters
{
    public class CloneController : BaseController
    {

        public float relativeSpeed;
        protected override void Start()
        {
            base.Start();
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
            var speedAdjustedTime = charAction.time * (1/relativeSpeed);

            switch (charAction.action)
            {
                case Actions.Move:
                    var move = UsefulMethods.StringToVector3(charAction.parameter);
                    StartCoroutine(WaitMoveCharacterViaAction(move, speedAdjustedTime));
                    break;
                case Actions.RotateCharacter:
                    var charRot = float.Parse(charAction.parameter);
                    StartCoroutine(WaitRotateCharacterViaAction(charRot, speedAdjustedTime));
                    break;
                case Actions.Jump:
                    StartCoroutine(WaitJumpCharacterViaAction(speedAdjustedTime));
                    break;
                case Actions.RotateCamera:
                    var camRot = float.Parse(charAction.parameter);
                    StartCoroutine(WaitRotateCameraViaAction(camRot, speedAdjustedTime));
                    break;
                case Actions.Shoot:
                    StartCoroutine(WaitShootGunViaAction(speedAdjustedTime));
                    break;
                case Actions.SpawnClone:
                    StartCoroutine(WaitSpawnClone(speedAdjustedTime));
                    break;
                default:
                    Logging.Log(charAction.action + " is not a valid Method Name");
                    break;
            }
        }

        private IEnumerator WaitSpawnClone(float timeToOccur)
        {
            yield return new WaitForSeconds(timeToOccur - Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
            SpawnClone();
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
            MoveCharacterForward(new Vector3(move.x, 0, move.z));
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
}
