using System.Collections.Generic;

using Unity.Burst;


namespace ClockBlockers.MapData.Pathfinding
{
	[BurstCompile]
	public readonly struct MultiPathRequest
	{
		public readonly IPathRequester pathRequester;

		public readonly List<PathfindingMarker> markers;
		
		public readonly float maxJumpHeight;

		public MultiPathRequest(IPathRequester pathRequester, List<PathfindingMarker> markers, float maxJumpHeight)
		{
			this.pathRequester = pathRequester;

			this.markers = markers;

			this.maxJumpHeight = maxJumpHeight;
		}
	}
}