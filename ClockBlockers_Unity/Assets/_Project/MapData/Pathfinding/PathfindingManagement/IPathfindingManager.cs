using System.Collections.Generic;
using System.Collections.ObjectModel;

using ClockBlockers.AI;
using ClockBlockers.ReplaySystem;

using UnityEngine;


namespace ClockBlockers.MapData.Pathfinding.PathfindingManagement
{
	public interface IPathfindingManager
	{
		void RequestPath(IPathRequester pathRequester, PathfindingMarker marker1, PathfindingMarker marker2, float maxJumpHeight);
		void RequestPath(IPathRequester pathRequester, Vector3 point1, Vector3 point2, float maxJumpHeight);
		void FindPaths();
		void RequestMultiPath(IPathRequester pathRequester, Vector3 startPoint, IEnumerable<Vector3> listOfPoints, float maxJumpHeight);
		void RequestMultiPath(IPathRequester pathRequester, PathfindingMarker startMarker, List<PathfindingMarker> listOfMarkers, float maxJumpHeight);
	}
}