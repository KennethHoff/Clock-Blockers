using System;
using System.Collections.Generic;


namespace ClockBlockers.MapData.Pathfinding
{
	public interface IPathfinder
	{
		List<Node> OpenList { get; set; }
	}
}