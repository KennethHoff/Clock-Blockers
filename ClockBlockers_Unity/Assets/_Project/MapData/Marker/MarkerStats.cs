using System;


namespace ClockBlockers.MapData.Marker
{
	// DONE: Find out why these don't save across editor reloads
		// Apparently, 'Readonly' makes them non-serialized?
	[Serializable]
	public class MarkerStats
	{
		// TODO: New name for this class.
		
		public PathfindingMarker marker;
		public float distance;

		public MarkerStats(PathfindingMarker marker, float distance)
		{
			this.marker = marker;
			this.distance = distance;
		}
	}
}