using System.Collections.Generic;

using UnityEngine;


namespace ClockBlockers.MapData
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
			
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>(grid.NumberOfColumns * grid.NumberOfRows);
			
			
			for (var i = 0; i <= grid.NumberOfColumns; i ++)
			{
				CreateMarkerRow(i);
			}
		}

		private void CreateMarkerRow(int i)
		{
			float xPos = grid.XStartPos + (grid.xDistanceBetweenMarkers * i);

			Transform newRow = new GameObject("Row " + i).transform;

			newRow.position = new Vector3(xPos, 0, 0);

			newRow.SetParent(gridTransform);

			var createdNothing = true;

			for (var j = 0; j <= grid.NumberOfRows; j++)
			{
				if (CreateMarker(j, xPos, newRow)) createdNothing = false;
			}
			if (createdNothing) DestroyImmediate(newRow.gameObject);
		}

		private bool CreateMarker(int j, float xPos, Transform newRow)
		{
			float zPos = grid.ZStartPos + (grid.zDistanceBetweenMarkers * j);
			var markerPos = new Vector3(xPos, grid.drawHeightAboveFloor + grid.nodeScale/2, zPos);
	
			if (!grid.createMarkerNearOrInsideCollisions && Physics.CheckBox(markerPos, grid.minimumOpenAreaAroundMarkers * 0.5f)) return false;
			InstantiateMarker("Column " + j, markerPos, newRow);
			return true;
		}
		
		private void InstantiateMarker(string markerName, Vector3 markerPos, Transform parent)
		{
			PathfindingMarker newMarker = Instantiate(grid.markerPrefab, markerPos, Quaternion.identity, parent);
			newMarker.Grid = grid;

			newMarker.name = markerName;
			grid.markers.Add(newMarker);
		}
	}
}