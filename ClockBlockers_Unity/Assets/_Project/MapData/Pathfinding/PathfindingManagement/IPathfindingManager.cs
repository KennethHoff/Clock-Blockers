using UnityEngine;


namespace ClockBlockers.MapData.Pathfinding.PathfindingManagement
{
	public interface IPathfindingManager
	{
		void RequestPath(IPathRequester pathRequester, PathfindingMarker marker1, PathfindingMarker marker2, float maxJumpHeight);
		void RequestPath(IPathRequester pathRequester, Vector3 point1, Vector3 point2, float maxJumpHeight);
		void FindPaths();
	}
}