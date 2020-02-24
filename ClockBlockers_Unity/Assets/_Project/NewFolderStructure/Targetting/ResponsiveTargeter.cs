using System.Collections.Generic;

using ClockBlockers.DataStructures;

using UnityEngine;


namespace ClockBlockers.Targetting
{
	public class ResponsiveTargeter : MonoBehaviour, ITargeter
	{
		private List<Transform> _interactables;

		[SerializeField]
		private float threshold;

		private Transform _closestTarget;

		private float _bestLookPercentage;

		private void Start()
		{
			_interactables = new List<Transform>();
			Transform[] allTransforms = FindObjectsOfType<Transform>();
			foreach (Transform t in allTransforms)
			{
				if (t.TryGetComponent(out IInteractable _))
				{
					_interactables.Add(t);
				}
			}
		}

		public IInteractable GetInteractableFromRay(Ray ray, float range)
		{
			foreach (Transform t in _interactables)
			{
				Vector3 vector1 = ray.direction;
				Vector3 vector2 = t.position - ray.origin;
				
				if (vector2.magnitude > range) continue;
				
				float lookPercentage = Vector3.Dot(vector1.normalized, vector2.normalized);

				if (!(lookPercentage > _bestLookPercentage)) continue;
				_bestLookPercentage = lookPercentage;
				_closestTarget = t;
			}

			return _closestTarget.GetComponent<IInteractable>();
		}
	}
}