using UnityEngine;

namespace ClockBlockers.MapData
{
	public abstract class IntervalMarkerGeneratorBase : MarkerGeneratorBase
	{
		[Range(MinDistance, MaxDistance)]
		public int xDistanceBetweenMarkers;

		[Range(MinDistance, MaxDistance)]
		public int zDistanceBetweenMarkers;
		
		protected const float MinDistance = 1f;
		protected const float MaxDistance = 100f;
		protected float MarkerSizeAdjustedXStartPos => Mathf.Ceil(-grid.XLength / 2 + XDistanceBetweenMarkers/2);
		protected float MarkerSizeAdjustedZStartPos => Mathf.Floor(grid.ZLength / 2 - ZDistanceBetweenMarkers/2);
		private float XDistanceBetweenMarkers => xDistanceBetweenMarkers;
		private float ZDistanceBetweenMarkers => zDistanceBetweenMarkers;
		protected int NumberOfColumns => Mathf.FloorToInt(grid.XLength / xDistanceBetweenMarkers)-1;
		protected int NumberOfRows => Mathf.FloorToInt(grid.ZLength / zDistanceBetweenMarkers)-1;

		public override void ResetAllMarkerGizmos()
		{
			foreach (PathfindingMarker marker in grid.markers)
			{
				marker.scale = grid.nodeScale;
				marker.PickAColor();
			}
		}
	}
}