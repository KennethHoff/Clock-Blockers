using System.Collections.Generic;

using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;

namespace ClockBlockers.MapData.Pathfinding.PathfindingManagement
{
	[ExecuteAlways][BurstCompile]
	internal class AStarPathfindingManager : MonoBehaviour, IPathfindingManager
	{
		[SerializeField]
		private int pathfindingChecksPerFramePerFinder = 100;

		private readonly List<PathRequest> pathRequests = new List<PathRequest>();

		private PathfindingGrid grid;


		private void OnValidate()
		{
			if (pathfindingChecksPerFramePerFinder < 1) pathfindingChecksPerFramePerFinder = 1;
		}

		private void FixedUpdate()
		{
			FindPaths();
		}

		private void Awake()
		{
			grid = GetComponent<PathfindingGrid>();
		}

		public void RequestPath(IPathRequester pathRequester, PathfindingMarker startMarker, PathfindingMarker endMarker, float maxJumpHeight)
		{
			pathRequests.Add(new PathRequest(pathRequester, startMarker, endMarker, maxJumpHeight));
		}

		public void RequestPath(IPathRequester pathRequester, Vector3 point1, Vector3 point2, float maxJumpHeight)
		{
			PathfindingMarker marker1 = grid.FindNearestMarker(point1);
			PathfindingMarker marker2 = grid.FindNearestMarker(point2);
			
			RequestPath(pathRequester, marker1, marker2, maxJumpHeight);
		}

		public void FindPaths()
		{
			FindPathsViaCoroutines();
		}

		private void FindPathsViaCoroutines()
		{
			int pathRequestsCount = pathRequests.Count;
			if (pathRequestsCount == 0) return;

			Logging.Log($"Searching for {pathRequestsCount} paths");
			
			foreach (PathRequest pathRequest in pathRequests)
			{
				
				var newPathfinder = AStarPathFinder.CreateInstance(pathRequest, pathfindingChecksPerFramePerFinder);
				pathRequest.pathRequester.CurrentPathfinder = newPathfinder;
				
#if UNITY_EDITOR
				if (!Application.isPlaying)
				{
					Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(newPathfinder.FindPathCoroutine(), this);
				}
				else
				{
					StartCoroutine(newPathfinder.FindPathCoroutine());
				}
#else
				StartCoroutine(newPathfinder.FindPathCoroutine());
#endif
			}
			
			pathRequests.Clear();
		}

	}
}