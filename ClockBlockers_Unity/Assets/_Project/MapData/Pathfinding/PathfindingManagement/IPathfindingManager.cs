using System.Collections.Generic;

using UnityEngine;


namespace ClockBlockers.MapData.Pathfinding.PathfindingManagement
{
	public interface IPathfindingManager
	{
		void RequestPath(IPathRequester pathRequester, PathfindingMarker startMarker, PathfindingMarker endMarker, float maxJumpHeight);
		void RequestPath(IPathRequester pathRequester, Vector3 startPoint, Vector3 endPoint, float maxJumpHeight);
		void RequestPath(IPathRequester pathRequester, Vector3 startPoint, PathfindingMarker endMarker, float maxJumpHeight);
		void FindPaths();
		void RequestMultiPath(IPathRequester pathRequester, Vector3 startPoint, IEnumerable<Vector3> listOfPoints, float maxJumpHeight);
		void RequestMultiPath(IPathRequester pathRequester, PathfindingMarker startMarker, List<PathfindingMarker> listOfMarkers, float maxJumpHeight);
	}
}