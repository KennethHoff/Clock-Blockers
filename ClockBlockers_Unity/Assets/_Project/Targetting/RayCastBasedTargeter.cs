using System;

using ClockBlockers.ToBeMoved.DataStructures;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.Targetting
{
	[BurstCompile]
	internal class RayCastBasedTargeter : MonoBehaviour, ITargeter
	{
		public Tuple<IInteractable, RaycastHit> GetInteractableFromRay(Ray ray, float range)
		{
			if (!RayCaster.CastRay(ray, range, out RaycastHit hit)) return null;
			var interactable = hit.transform.GetComponent<IInteractable>();
			return interactable == null ? null : new Tuple<IInteractable, RaycastHit>(interactable, hit);
		}
	}
}