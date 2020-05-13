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

		protected Queue<PathfindingMarker> currentPath;

		protected PathfindingMarker currentMarker;

		// In relation to pathfinding - 'How high can I jump'
		[SerializeField]
		protected float highestReachableRelativeAltitude;

		public IPathfindingManager pathfindingManager;

		protected List<PathfindingMarker>[] _workInProgressPath;


		// public IPathfinder CurrentPathfinder { get; set; }
		public IPathfinder[] CurrentPathfinders { get; set; }
		
		public bool OnAPath => currentPath != null || CurrentPathfinders != null || (currentMarker != null && HorizontalDistanceToCurrentPathMarker() > characterMovement.MoveSpd * 0.2f);

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
		
		public void PathCallback(IEnumerable<PathfindingMarker> pathFinderPath, IPathfinder pathfinder)
		{
			if (CurrentPathfinders == null || !CurrentPathfinders.Contains(pathfinder)) return;
			
			currentPath = new Queue<PathfindingMarker>(pathFinderPath);
			GetNextMarkerInPath();
			Logging.Log($"{name} successfully received their path");

		}

		public void PathCallback(List<PathfindingMarker> pathFinderPath, int pathfinderIndex)
		{
			if (CurrentPathfinders?[pathfinderIndex] == null) return;

			CurrentPathfinders[pathfinderIndex] = null;

			Logging.Log($"Got a path callback from pathfinder #{pathfinderIndex}!");

			_workInProgressPath[pathfinderIndex] = pathFinderPath;

			MergeWorkInProgressPaths();
		}

		private void MergeWorkInProgressPaths()
		{
			if (CurrentPathfinders.Any(pathfinder => pathfinder != null)) return;

			currentPath = new Queue<PathfindingMarker>(_workInProgressPath.SelectMany(x => x));
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

		public void Inject(IPathfindingManager currPathfindingManager)
		{
			pathfindingManager = currPathfindingManager;
		}

		protected void GetNextMarkerInPath()
		{
			currentMarker = currentPath.Dequeue();
			// Logging.Log($"Current marker is now {currentMarker.name}");
		}

		public abstract void RequestMultiPath(List<Vector3> listOfPoints);
	}
}