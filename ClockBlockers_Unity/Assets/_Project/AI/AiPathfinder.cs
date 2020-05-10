using System.Collections;
using System.Collections.Generic;

using ClockBlockers.Characters;
using ClockBlockers.MapData;
using ClockBlockers.MapData.Pathfinding;
using ClockBlockers.MapData.Pathfinding.PathfindingManagement;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;

namespace ClockBlockers.AI
{
	[BurstCompile]
	public abstract class AiPathfinder : MonoBehaviour, IPathRequester
	{
		protected CharacterMovementNew characterMovement;

		protected List<PathfindingMarker> currentPath;

		protected PathfindingMarker CurrentPathMarker => currentPath[currentPathIndex];


		protected int currentPathIndex;

		// In relation to pathfinding - 'How high can I jump'
		protected float highestReachableRelativeAltitude;

		public IPathfindingManager pathfindingManager;

		
		public IPathfinder CurrentPathfinder { get; set; }

		protected virtual void Awake()
		{
			characterMovement = GetComponent<CharacterMovementNew>();
		}

		protected float DistanceToCurrentPathMarker()
		{
			return Vector3.Distance(CurrentPathMarker.transform.position, transform.position);
		}

		public abstract void MoveTowardsNextWaypoint();

		public void PathCallback(List<PathfindingMarker> pathFinderPath)
		{
			currentPath = pathFinderPath;
			Logging.Log($"{name} successfully received their path");
		}

		protected void IncrementCurrentPathIndex()
		{
			currentPathIndex++;
		}


		public virtual void RunPathfinding()
		{
			if (CurrentPathMarker == null) return;
			
			if (DistanceToCurrentPathMarker() < characterMovement.MoveSpd) IncrementCurrentPathIndex();;
		}

		public abstract void RequestPath(Vector3 destination);

		public void EndCurrentPath()
		{
			currentPath = null;
			currentPathIndex = 0;
		}
	}
}