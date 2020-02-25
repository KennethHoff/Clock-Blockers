using System;

using ClockBlockers.DataStructures;

using UnityEngine;


namespace ClockBlockers.Targetting
{
	internal class RayCastBasedTargeter : MonoBehaviour, ITargeter
	{
		public Tuple<IInteractable, RaycastHit> GetInteractableFromRay(Ray ray, float range)
		{
			if (!Physics.Raycast(ray, out RaycastHit hit, range)) return null;
			var interactable = hit.transform.GetComponent<IInteractable>();
			return interactable == null ? null : new Tuple<IInteractable, RaycastHit>(interactable, hit);
		}
	}
}