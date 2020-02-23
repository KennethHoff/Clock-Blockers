using System;
using System.Collections.Generic;
using ClockBlockers.Characters;
using ClockBlockers.Components;
using ClockBlockers.DataStructures;
using ClockBlockers.Utility;
using UnityEngine;

namespace ClockBlockers.Environment
{
    public class ShootingTarget1 : MonoBehaviour, IInteractable
    {

        public List<Tuple<Vector3, BaseController>> hitList;

        public void OnHit(DamagePacket damagePacket, Vector3 hitPosition)
        {
            //var currentPos = transform.position;
            //var xDiff = hitPosition.x - currentPos.x;
            //var yDiff = hitPosition.y - currentPos.y;
            //var zDiff = hitPosition.z - currentPos.z;

            //Logging.Log("x Difference: " + xDiff.ToString("F10") +
            //          ". y Difference: " + yDiff.ToString("F10") +
            //          ". Z difference: " + zDiff.ToString("F10") +
            //          " | " + damagePacket.source.name);


            BaseController controller = damagePacket.source.transform.GetComponent<GunController>().holder;

            if (hitList == null)
            {
                hitList = new List<Tuple<Vector3, BaseController>>();
            }

            var sameAsPreviousShot = true;
            var ownerAlreadyExists = false;

            float tupleDiff = 0;

            foreach ((Vector3 position, BaseController character) in hitList)
            {
                if (character.GetInstanceID() == controller.GetInstanceID())
                {
                    ownerAlreadyExists = true;
                    break;
                }

                tupleDiff = hitPosition.x - position.x + hitPosition.y - position.y + hitPosition.z - position.z;
                if (Math.Abs(tupleDiff) > 0.000001f)
                {
                    sameAsPreviousShot = false;
                }
            }

            if (ownerAlreadyExists)
            {
                //Logging.Log("Already shot on this target!");
                return;
            }
            if (hitList.Count < 1)
            {
                hitList.Add(new Tuple<Vector3, BaseController>(hitPosition, controller));
            }

            if (sameAsPreviousShot)
            {
                Logging.Log("Same as all previous shot on target: " + transform.parent.name);
            }
            else
            {
                Logging.Log("Not same as all previous shot on target: " + transform.parent.name + ". Off by: " + tupleDiff);
            }
        }
    }
}