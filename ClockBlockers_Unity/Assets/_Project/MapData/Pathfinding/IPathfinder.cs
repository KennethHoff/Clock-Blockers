﻿using System.Collections.Generic;

using UnityEngine;


namespace ClockBlockers.MapData.Pathfinding
{
	public interface IPathfinder
	{
		List<PathfindingMarker> GetPath(PathfindingMarker startMarker, PathfindingMarker endMarker);
	}
}