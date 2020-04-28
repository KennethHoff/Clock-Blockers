using System;


namespace ClockBlockers.MapData
{
	// TODO: Find out why these don't save across editor reloads
	[Serializable]
	public class MarkerStats
	{
		// TODO: New name for this class.
		public static MarkerStats CreateInstance(PathfindingMarker marker, float distanceAway)
		{
			return new MarkerStats(marker, distanceAway);
		}
		
		public readonly PathfindingMarker marker;
		public readonly float distance;

		private MarkerStats(PathfindingMarker marker, float distance)
		{
			this.marker = marker;
			this.distance = distance;
		}

	}
}