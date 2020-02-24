using ClockBlockers.DataStructures;

using UnityEngine;


namespace ClockBlockers.Targetting
{
	internal interface ITargeter
	{
		IInteractable GetInteractableFromRay(Ray ray);
	}
}