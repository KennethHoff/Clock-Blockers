using System.Collections.Generic;


namespace ClockBlockers.MapData.Pathfinding
{
	public interface IPathfinder
	{
		int PathfinderIndex { get; set; }
		List<Node> OpenList { get; set; }
		void EndPreemptively();
	}
}