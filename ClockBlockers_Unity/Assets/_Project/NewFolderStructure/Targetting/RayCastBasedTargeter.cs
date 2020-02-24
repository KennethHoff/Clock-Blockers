using System;

using ClockBlockers.DataStructures;

using UnityEngine;


namespace ClockBlockers.Targetting
{
	internal class RayCastBasedTargeter : MonoBehaviour, ITargeter
	{
		public Tuple<IInteractable, RaycastHit> GetInteractableFromRay(Ray ray)
		{
			if (!Physics.Raycast(ray, out RaycastHit hit)) return null;
			var interactable = hit.transform.GetComponent<IInteractable>();
			if (interactable == null) return null;
			return new Tuple<IInteractable, RaycastHit>(interactable, hit);
		}
	}
}