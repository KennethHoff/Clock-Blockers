using System.Collections.Generic;

using ClockBlockers.MapData.Pathfinding;

namespace ClockBlockers.MapData
{
	public interface IPathRequester
	{
		void PathCallback(List<PathfindingMarker> pathFinderPath);

		// Currently (as of 08/05/2020) this is a temporary valuable used for showing the path as it's being created (Exclusively inside the UNITY_EDITOR at that - it does look really nice though)
		IPathfinder CurrentPathfinder { get; set; }
	}
}