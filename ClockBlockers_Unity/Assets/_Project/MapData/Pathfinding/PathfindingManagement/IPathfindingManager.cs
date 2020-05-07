using System.Collections.Generic;


namespace ClockBlockers.MapData.Pathfinding.PathfindingManagement
{
	public interface IPathfindingManager
	{
		List<PathfindingMarker> RequestPath(IPathRequester pathRequester, PathfindingMarker marker1, PathfindingMarker marker2);
		void FindPaths();
	}
}