﻿using System.Collections.Generic;

using Unity.Burst;


namespace ClockBlockers.MapData.Pathfinding
{
	[BurstCompile]
	public readonly struct PathRequest
	{
		public PathRequest(IPathRequester pathRequester, PathfindingMarker startMarker, PathfindingMarker endMarker, float maxJumpHeight)
		{
			this.pathRequester = pathRequester;
			this.startMarker = startMarker;
			this.endMarker = endMarker;
			this.maxJumpHeight = maxJumpHeight;
		}

		public readonly IPathRequester pathRequester;
		public readonly PathfindingMarker startMarker;
		public readonly PathfindingMarker endMarker;
		public readonly float maxJumpHeight;
	}
}