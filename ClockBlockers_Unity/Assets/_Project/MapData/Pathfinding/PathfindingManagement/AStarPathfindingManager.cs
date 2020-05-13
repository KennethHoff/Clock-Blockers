using System.Collections.Generic;
using System.Linq;

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

		private List<PathRequest> _pathRequests = new List<PathRequest>();

		private List<MultiPathRequest> _multiPathRequests = new List<MultiPathRequest>();

		private PathfindingGrid _grid;

		private const int MinimumChecksPerFramePerFinder = 1;

		private void OnValidate()
		{
			if (pathfindingChecksPerFramePerFinder < MinimumChecksPerFramePerFinder) pathfindingChecksPerFramePerFinder = MinimumChecksPerFramePerFinder;
		}

		private void FixedUpdate()
		{
			FindPaths();
		}

		private void Awake()
		{
			_grid = GetComponent<PathfindingGrid>();
		}

		public void RequestPath(IPathRequester pathRequester, PathfindingMarker startMarker, PathfindingMarker endMarker, float maxJumpHeight)
		{
			_pathRequests.Add(new PathRequest(pathRequester, startMarker, endMarker, maxJumpHeight));
		}

		public void RequestPath(IPathRequester pathRequester, Vector3 point1, Vector3 point2, float maxJumpHeight)
		{
			PathfindingMarker marker1 = _grid.FindNearestMarker(point1);
			PathfindingMarker marker2 = _grid.FindNearestMarker(point2);
			
			Logging.Log($"Trying to move from {marker1.name} to {marker2.name}!");
			
			RequestPath(pathRequester, marker1, marker2, maxJumpHeight);
		}

		public void FindPaths()
		{
			FindPathsViaCoroutines();
		}

		public void RequestMultiPath(IPathRequester pathRequester, Vector3 startPoint, IEnumerable<Vector3> listOfPoints, float maxJumpHeight)
		{
			PathfindingMarker startMarker = _grid.FindNearestMarker(startPoint);
			List<PathfindingMarker> listOfMarkersToGoThrough = listOfPoints.Select(point => _grid.FindNearestMarker(point)).ToList();

			Logging.Log($"Requesting path to {listOfMarkersToGoThrough.Last().name} from {name} via {listOfMarkersToGoThrough.Count - 1} other markers");

			RequestMultiPath(pathRequester, startMarker, listOfMarkersToGoThrough, maxJumpHeight);
		}

		public void RequestMultiPath(IPathRequester pathRequester, PathfindingMarker startMarker, List<PathfindingMarker> listOfMarkers, float maxJumpHeight)
		{
			_multiPathRequests.Add(new MultiPathRequest(pathRequester, listOfMarkers, maxJumpHeight));
		}

		private void FindPathsViaCoroutines()
		{
			int pathRequestsCount = _pathRequests.Count;
			if (pathRequestsCount != 0)
			{
				Logging.Log($"Searching for {pathRequestsCount} paths");
			
				foreach (PathRequest pathRequest in _pathRequests)
				{
					const int defaultIndex = 0;
					var newPathfinder = AStarPathFinder.CreateInstance(pathRequest, pathfindingChecksPerFramePerFinder, defaultIndex);
					pathRequest.pathRequester.CurrentPathfinders = new IPathfinder[] {newPathfinder};
				
					StartPathfinderCoroutine(newPathfinder);
				}
				_pathRequests.Clear();
			}
			
			
			int multiPathRequestsCount = _multiPathRequests.Count;
			if (multiPathRequestsCount != 0)
			{
				Logging.Log($"Searching for {multiPathRequestsCount} multiPaths");

				foreach (MultiPathRequest multiPathRequest in _multiPathRequests)
				{
					int requestAmount = multiPathRequest.markers.Count-1;
					Logging.Log($"Multipath has {requestAmount} number of requests");

					IPathRequester pathRequester = multiPathRequest.pathRequester;

					for (var index = 0; index < requestAmount; index++)
					{
						PathfindingMarker currStartMarker = multiPathRequest.markers[index];
						PathfindingMarker currEndMarker = multiPathRequest.markers[index+1];

						var newPathRequest = new PathRequest(pathRequester, currStartMarker, currEndMarker, multiPathRequest.maxJumpHeight);

						var newPathfinder = AStarPathFinder.CreateInstance(newPathRequest, pathfindingChecksPerFramePerFinder, index);

						pathRequester.CurrentPathfinders[index] = newPathfinder;

						StartPathfinderCoroutine(newPathfinder);
					}
				}
			}

			_multiPathRequests.Clear();
		}

		private void StartPathfinderCoroutine(AStarPathFinder newPathfinder)
		{
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
	}
}