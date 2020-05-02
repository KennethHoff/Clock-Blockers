using System.Collections.Generic;

using UnityEngine;


namespace ClockBlockers.MapData
{
	[ExecuteInEditMode]
	public class AlongFloorMarkerGenerator : IntervalMarkerGeneratorBase
	{
		public override void GenerateAllMarkers()
		{
			if (xDistanceBetweenMarkers < MinDistance || zDistanceBetweenMarkers < MinDistance) return; 
			
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>(NumberOfColumns * NumberOfRows);
			
			
			for (var i = 0; i <= NumberOfColumns; i ++)
			{
				CreateMarkerRow(i);
			}
		}

		public override void ClearMarkers()
		{
			foreach (PathfindingMarker marker in grid.markers)
			{
				if (marker == null) continue;

				DestroyImmediate(marker.transform.parent.gameObject);
			}
			grid.markers.Clear();
		}

		private void CreateMarkerRow(int i)
		{
			float xPos = MarkerSizeAdjustedXStartPos + (xDistanceBetweenMarkers * i);

			Transform newRow = new GameObject("Row " + i).transform;

			newRow.position = new Vector3(xPos, 0, 0);

			newRow.SetParent(gridTransform);

			var createdNothing = true;

			for (var j = 0; j <= NumberOfRows; j++)
			{
				if (CreateMarker(j, xPos, newRow)) createdNothing = false;
			}
			if (createdNothing) DestroyImmediate(newRow.gameObject);
		}

		private bool CreateMarker(int j, float xPos, Transform newRow)
		{
			float zPos = MarkerSizeAdjustedZStartPos + (zDistanceBetweenMarkers * j);
			var markerPos = new Vector3(xPos, grid.drawHeightAboveFloor + grid.nodeScale/2, zPos);
	
			if (!grid.createMarkerNearOrInsideCollisions && Physics.CheckBox(markerPos, grid.minimumOpenAreaAroundMarkers * 0.5f)) return false;
			InstantiateMarker("Column " + j, markerPos, ref newRow);
			return true;
		}
		
		private void InstantiateMarker(string markerName, Vector3 markerPos, ref Transform parent)
		{
			var newMarker = PathfindingMarker.CreateInstance(markerName, ref grid, ref markerPos, ref parent, ref grid.creationHeightAboveFloor);
			newMarker.Grid = grid;

			newMarker.name = markerName;
			grid.markers.Add(newMarker);
		}
	}
}