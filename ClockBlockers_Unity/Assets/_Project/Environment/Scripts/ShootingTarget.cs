using System;
using System.Collections.Generic;

using ClockBlockers.Characters;
using ClockBlockers.ToBeMoved.DataStructures;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.Environment
{
	[BurstCompile]
	internal class ShootingTarget : MonoBehaviour, IInteractable
	{
		private List<Tuple<Vector3, Character>> HitList { get; set; }

		public void OnHit(DamagePacket damagePacket, Vector3 hitPosition)
		{
			
			Vector3 currentPos = transform.position;
			float xDistFromCenter = hitPosition.x - currentPos.x;
			float yDistFromCenter = hitPosition.y - currentPos.y;
			float zDistFromCenter = hitPosition.z - currentPos.z;
			
			Logging.Log("x Difference: " + xDistFromCenter.ToString("F10") +
			". y Difference: " + yDistFromCenter.ToString("F10") +
			". Z difference: " + zDistFromCenter.ToString("F10") +
			" | " + damagePacket.source.name);


			// if (HitList == null)
			// {
				// HitList = new List<Tuple<Vector3, Character>>();
			// }

			// var sameAsPreviousShot = true;
			// var ownerAlreadyExists = false;

			// float tupleDiff = 0;

			// foreach ((Vector3 position, Character character) in HitList)
			// {
			// 	if (character.GetInstanceID() == controller.GetInstanceID())
			// 	{
			// 		ownerAlreadyExists = true;
			// 		break;
			// 	}
			//
			// 	tupleDiff = hitPosition.x - position.x + hitPosition.y - position.y + hitPosition.z - position.z;
			// 	if (Math.Abs(tupleDiff) > 0.000001f)
			// 	{
			// 		sameAsPreviousShot = false;
			// 	}
			// }

			// if (ownerAlreadyExists)
			// {
				// Logging.Log("Already shot on this target!");
				// return;
			// }

			// if (HitList.Count < 1)
			// {
				// HitList.Add(new Tuple<Vector3, Character>(hitPosition, controller));
			// }

			// if (sameAsPreviousShot)
			// {
				// Logging.instance.Log("Same as all previous shot on target: " + transform.parent.name);
			// }
			// else
			// {
				// Logging.instance.Log("Not same as all previous shot on target: " + transform.parent.name + ". Off by: " +
				                     // tupleDiff);
			// }
		}
	}
}