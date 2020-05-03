using System.Collections.Generic;

using ClockBlockers.Utility;

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

		public sealed override void GenerateMarkerAdjacencies()
		{
			foreach (PathfindingMarker marker in grid.markers)
			{
				marker.GetAdjacentMarkersFromGrid();
			}
		}
		
		public sealed override void ClearMarkers()
		{
			foreach (PathfindingMarker marker in grid.markers)
			{
				if (marker == null) continue;
				DestroyImmediate(marker.transform.parent.gameObject);
			}
			grid.markers.Clear();
		}

		public sealed override void GenerateAllMarkers()
		{
			if (xDistanceBetweenMarkers < MinDistance || zDistanceBetweenMarkers < MinDistance)
			{
				Logging.LogWarning("X/Z Distance Between Markers is unset.");
				return;
			} 
			
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>(NumberOfColumns * NumberOfRows);
			
			
			for (var i = 0; i <= NumberOfColumns; i ++)
			{
				CreateMarkerColumn(i);
			}
		}
		
		protected void CreateMarkerColumn(int i)
		{
			float xPos = MarkerSizeAdjustedXStartPos + (xDistanceBetweenMarkers * i);

			Transform newColumn = new GameObject("Column " + i).transform;

			newColumn.position = new Vector3(xPos, 0, 0);

			newColumn.SetParent(gridTransform);

			var createdNothing = true;

			for (var j = 0; j <= NumberOfRows; j++)
			{
				if (CreateMarker(j, xPos, newColumn)) createdNothing = false;
				else Logging.Log("Did not create any markers on column " + i + ", row " + j);
			}

			if (!createdNothing) return;
			DestroyImmediate(newColumn.gameObject);
			Logging.Log("Did not create any Markers on column " + i);
		}

		protected abstract bool CreateMarker(int i, float xPos, Transform newColumn);

		protected void InstantiateMarker(string markerName, Vector3 markerPos, ref Transform parent)
		{
			PathfindingMarker.CreateInstance(markerName, ref grid, ref markerPos, ref parent, ref creationHeightAboveFloor);
		}
	}
}