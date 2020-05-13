using System;

using ClockBlockers.ToBeMoved.DataStructures;

using UnityEngine;


namespace ClockBlockers.Targetting
{
	internal interface ITargeter
	{
		Tuple<IInteractable, RaycastHit> GetInteractableFromRay(Ray ray, float range);
	}
}