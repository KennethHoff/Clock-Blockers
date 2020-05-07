using System.Collections.Generic;


namespace ClockBlockers.MapData.Pathfinding
{
	public interface IPathfinder
	{
		// void FindPath();
		List<Node> OpenList { get; set; }
	}
}