using System;
using System.Collections.Generic;
using System.Linq;

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

		protected internal Queue<PathfindingMarker> currentPath;

		protected PathfindingMarker currentMarker;

		// In relation to pathfinding - 'How high can I jump'
		[SerializeField]
		protected float highestReachableRelativeAltitude;

		public IPathfindingManager pathfindingManager;

		protected List<PathfindingMarker>[] workInProgressPath;
		
		// Remove this when you get a non-marker-based pathfinder
		protected PathfindingGrid grid;
		
		
		[NonSerialized]
		public bool hasCompletedAPath = false;


		// public IPathfinder CurrentPathfinder { get; set; }
		public IPathfinder[] CurrentPathfinders { get; set; }
		
		public bool NotOnAPath => currentPath == null || CurrentPathfinders == null || (currentMarker != null && HorizontalDistanceToCurrentPathMarker() <= characterMovement.MoveSpd * 0.05f);
		public bool OnAPath => !NotOnAPath;

		protected virtual void Awake()
		{
			characterMovement = GetComponent<CharacterMovementNew>();
		}

		protected float HorizontalDistanceToCurrentPathMarker()
		{
			var currMarkerPos3D = currentMarker.transform.position;
			var currMarkerPos2D = new Vector2(currMarkerPos3D.x, currMarkerPos3D.z);

			var currPos3D = transform.position;
			var currPos2D = new Vector2(currPos3D.x, currPos3D.z);
			
			return Vector2.Distance(currMarkerPos2D, currPos2D);
		}

		public abstract void MoveTowardsNextWaypoint();
		
		public void PathCallback(List<PathfindingMarker> pathFinderPath, int pathfinderIndex)
		{
			Logging.Log($"Got a path callback from pathfinder #{pathfinderIndex}!");

			if (CurrentPathfinders == null) return;
			
			if (CurrentPathfinders[pathfinderIndex] == null)
			{
				Logging.LogWarning($"{name} Got Path Callback on {pathfinderIndex}, but there was already a path on that index");
				return;
			}

			CurrentPathfinders[pathfinderIndex] = null;

			workInProgressPath[pathfinderIndex] = pathFinderPath;

			MergeWorkInProgressPaths();
		}

		private void MergeWorkInProgressPaths()
		{
			if (CurrentPathfinders.Any(pathfinder => pathfinder != null)) return;
			if (workInProgressPath.Any(tempPath => tempPath == null))
			{
				Logging.LogWarning("WorkInProgressPath has a null path, but all pathfinders are complete!");
				return;
			}

			currentPath = new Queue<PathfindingMarker>(workInProgressPath.SelectMany(x => x));
			Logging.Log("Finalized Creation of paths");

			CurrentPathfinders = null;
		}


		public abstract void Tick();

		public abstract void RequestPath(Vector3 destination);
		
		public void EndCurrentPath()
		{
			currentPath = null;
			CurrentPathfinders = null;
		}

		public void Inject(IPathfindingManager currPathfindingManager, PathfindingGrid currGrid)
		{
			pathfindingManager = currPathfindingManager;
			grid = currGrid;
		}

		protected void GetNextMarkerInPath()
		{
			currentMarker = currentPath.Dequeue();
			// Logging.Log($"Current marker is now {currentMarker.name}");
		}

		public abstract void RequestMultiPath(List<Vector3> listOfPoints);

		public abstract PathfindingMarker RequestPathToRandomLocationWithinDistance(float distance);
	}
}