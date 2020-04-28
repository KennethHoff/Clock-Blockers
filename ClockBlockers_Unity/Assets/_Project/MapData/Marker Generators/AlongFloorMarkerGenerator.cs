using System.Collections.Generic;

using ClockBlockers.MapData.Grid;
using ClockBlockers.MapData.Marker;

using UnityEngine;


namespace ClockBlockers.MapData.Marker_Generators
{
	[ExecuteInEditMode]
	public class AlongFloorMarkerGenerator : MonoBehaviour, IMarkerGenerator
	{
		private PathfindingGrid grid;
		private Transform gridTransform;

		private void Awake()
		{
			grid = GetComponent<PathfindingGrid>();
			gridTransform = grid.transform;
		}



		public void GenerateAllMarkers()
		{
			if (grid.xDistanceBetweenMarkers < PathfindingGrid.MinDistance || grid.zDistanceBetweenMarkers < PathfindingGrid.MinDistance) return; 
			
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>();
			
			
			for (var i = 0; i <= grid.MaximumNumberOfColumns; i ++)
			{
				CreateMarkerRow(i);
			}
		}

		private void CreateMarkerRow(int i)
		{
			float xPos = grid.XStartPos + (grid.xDistanceBetweenMarkers * i);

			// PathfindingMarkerRow newRow = new GameObject("Row " + i).AddComponent<PathfindingMarkerRow>();
			// Transform rowTransform = newRow.transform;

			Transform rowTransform = new GameObject("Row " + i).transform;


			rowTransform.position = new Vector3(xPos, 0, 0);

			rowTransform.SetParent(gridTransform);

			var createdNothing = true;

			for (var j = 0; j <= grid.MaximumNumberOfRows; j++)
			{
				if (CreateMarker(j, xPos, ref rowTransform)) createdNothing = false;
			}
			if (createdNothing) DestroyImmediate(rowTransform.gameObject);
		}

		private bool CreateMarker(int j, float xPos, ref Transform rowTransform)
		{
			float zPos = grid.ZStartPos + (grid.zDistanceBetweenMarkers * j);
			var markerPos = new Vector3(xPos, grid.heightAboveFloor + grid.nodeScale/2, zPos);
	
			if (!grid.createMarkerNearOrInsideCollisions && Physics.CheckBox(markerPos, grid.minimumOpenAreaAroundMarkers * 0.5f)) return false;

			string markerName = "Column " + j;
			PathfindingMarker newMarker = PathfindingMarker.CreateInstance(markerName, ref markerPos, ref grid, ref rowTransform);
			
			grid.markers.Add(newMarker);

			return true;
		}
	}
}