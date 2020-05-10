using System;

using Unity.Burst;


namespace ClockBlockers.MapData
{
	// DONE: Find out why these don't save across editor reloads
		// Apparently, 'Readonly' makes them non-serialized?
	[Serializable][BurstCompile]
	public class MarkerStats
	{
		// TODO: New name for this class.
		
		public PathfindingMarker marker;
		public float yDistance;
		public AdjacencyDirection relativeDirection;

		public MarkerStats(PathfindingMarker marker, float yDist, AdjacencyDirection direction)
		{
			this.marker = marker;
			yDistance = yDist;
			relativeDirection = direction;
		}
	}
}