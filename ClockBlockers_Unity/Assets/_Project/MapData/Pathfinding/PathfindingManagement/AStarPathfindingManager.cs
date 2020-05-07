using System.Collections.Generic;

using Unity.Burst;
using Unity.EditorCoroutines.Editor;

using UnityEngine;

namespace ClockBlockers.MapData.Pathfinding.PathfindingManagement
{
	[ExecuteAlways][BurstCompile]
	internal class AStarPathfindingManager : MonoBehaviour, IPathfindingManager
	{
		[SerializeField]
		private int pathfindingChecksPerFramePerFinder = 100;

		
		private List<PathRequest> pathRequests = new List<PathRequest>();

		private void FixedUpdate()
		{
			FindPaths();
		}

		public List<PathfindingMarker> RequestPath(IPathRequester pathRequester, PathfindingMarker marker1, PathfindingMarker marker2)
		{
			pathRequests.Add(PathRequest.CreateInstance(pathRequester, marker1, marker2));

			return null;
		}
		
		public void FindPaths()
		{
			FindPathsViaCoroutines();
		}

		private void FindPathsViaCoroutines()
		{
			int pathRequestsCount = pathRequests.Count;
			if (pathRequestsCount == 0) return;
			
			foreach (PathRequest pathRequest in pathRequests)
			{
				var newPathfinder = AStarPathFinder.CreateInstance(pathRequest, pathfindingChecksPerFramePerFinder);
				pathRequest.pathRequester.CurrentPathfinder = newPathfinder;
#if UNITY_EDITOR
				EditorCoroutineUtility.StartCoroutine(newPathfinder.FindPathCoroutine(), this);
#else
				StartCoroutine(newPathfinder.FindPathCoroutine());
#endif
			}
			
			pathRequests.Clear();
		}

	}
}