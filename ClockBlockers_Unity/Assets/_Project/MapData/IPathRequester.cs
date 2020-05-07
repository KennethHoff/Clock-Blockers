using System.Collections.Generic;

using ClockBlockers.MapData.Pathfinding;


namespace ClockBlockers.MapData
{
	public interface IPathRequester
	{
		void PathCallback(List<PathfindingMarker> pathFinderPath);
		IPathfinder CurrentPathfinder { get; set; }
	}
}