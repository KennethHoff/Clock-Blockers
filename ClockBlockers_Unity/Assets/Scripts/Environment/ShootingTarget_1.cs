using System;
using System.Collections.Generic;
using DataStructures;
using UnityEngine;

namespace Environment
{
    public class ShootingTarget_1 : MonoBehaviour, IInteractable
    {

        public List<Tuple<Vector3, BaseController>> hitList;

        public void OnHit(DamagePacket damagePacket, Vector3 hitPosition)
        {
            //var currentPos = transform.position;
            //var xDiff = hitPosition.x - currentPos.x;
            //var yDiff = hitPosition.y - currentPos.y;
            //var zDiff = hitPosition.z - currentPos.z;

            //Debug.Log("x Difference: " + xDiff.ToString("F10") +
            //          ". y Difference: " + yDiff.ToString("F10") +
            //          ". Z difference: " + zDiff.ToString("F10") +
            //          " | " + damagePacket.source.name);


            var controller = damagePacket.source.transform.GetComponent<GunController>().holder;

            if (hitList == null)
                hitList = new List<Tuple<Vector3, BaseController>>();

            var sameAsPreviousShot = true;
            var ownerAlreadyExists = false;

            float tupleDiff = 0;

            foreach (var tuple in hitList)
            {
                if (tuple.Item2.GetInstanceID() == controller.GetInstanceID())
                {
                    ownerAlreadyExists = true;
                    break;
                }

                var tuplePos = tuple.Item1;
                tupleDiff = hitPosition.x - tuplePos.x + hitPosition.y - tuplePos.y + hitPosition.z - tuplePos.z;
                if (Math.Abs(tupleDiff) > 0.000001f)
                {
                    sameAsPreviousShot = false;
                }
            }

            if (ownerAlreadyExists)
            {
                //Debug.Log("Already shot on this target!");
                return;
            }
            if (hitList.Count < 1)
                hitList.Add(new Tuple<Vector3, BaseController>(hitPosition, controller));
            if (sameAsPreviousShot)
                Debug.Log("Same as all previous shot on target: " + transform.parent.name);
            else
                Debug.Log("Not same as all previous shot on target: " + transform.parent.name + ". Off by: " + tupleDiff);
        }


    }
}