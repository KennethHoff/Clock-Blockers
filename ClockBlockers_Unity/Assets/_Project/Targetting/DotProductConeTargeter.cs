using System;
using System.Collections.Generic;

using ClockBlockers.DataStructures;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.Targetting
{
	[BurstCompile]
	public class DotProductConeTargeter : MonoBehaviour, ITargeter
	{
		// ReSharper disable once IdentifierTypo
		private static List<Transform> _interactables;

		private float Threshold => threshold;

		private Transform _self;

		private Transform _closestTarget;

		[SerializeField]
		[Range(0, 1)]
		private float threshold = 0.99f;

		private float _distanceToClosestTarget;


		private void Awake()
		{
			_self = gameObject.transform;
		}

		private static void SetAllInteractables()
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

		public Tuple<IInteractable, RaycastHit> GetInteractableFromRay(Ray ray, float range)
		{
			_distanceToClosestTarget = range;
			SetAllInteractables();

			foreach (Transform t in _interactables)
			{
				// Don't hit yourself :|
				if (t == _self) continue;

				Vector3 rayVector = ray.direction;
				Vector3 rayToTargetVector = t.position - ray.origin;

				float distance = rayToTargetVector.magnitude;

				float lookPercentage = Vector3.Dot(rayVector.normalized, rayToTargetVector.normalized);

				if (lookPercentage < Threshold) continue;

				if (_distanceToClosestTarget <= distance) continue;

				// if (lookPercentage < _bestLookPercentage) return;

				_closestTarget = t;
				_distanceToClosestTarget = rayToTargetVector.magnitude;
			}

			if (_closestTarget == null) return null;

			// TODO: Implement a "Closest point" method; Basically, if you're aiming slightly above the head, then the point should be the top of the head.
			// If you're aiming slightly top-left, then the point should be the upper-left-most area of the object etc..

			// Currently it simply rays to the middle of the object.
			Physics.Linecast(ray.origin, _closestTarget.position, out RaycastHit rayCastHit);

			var result = new Tuple<IInteractable, RaycastHit>(_closestTarget.GetComponent<IInteractable>(), rayCastHit);

			_closestTarget = null;

			return result;
		}
	}
}