using System.Collections.Generic;


namespace ClockBlockers.MapData.Pathfinding.PathfindingManagement
{
	public interface IPathfindingManager
	{
		void RequestPath(IPathRequester pathRequester, PathfindingMarker marker1, PathfindingMarker marker2, float maxJumpHeight);
		void FindPaths();
	}
}