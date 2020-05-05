using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	[ExecuteInEditMode]
	public class AlongFloorMarkerGenerator : IntervalAutomatedMarkerGenerator
	{
		protected override bool CreateMarker(float xPos, int rowIndex, Transform newColumn, int columnIndex)
		{
			float zPos = MarkerSizeAdjustedZStartPos - (zDistanceBetweenCreatedMarkers * rowIndex); 
			
			var markerPos = new Vector3(xPos, creationHeightAboveFloor, zPos);

			if (!createMarkerNearOrInsideCollisions)
			{
				Vector3 checkPos = markerPos;
				checkPos.y += grid.minimumOpenAreaAroundMarkers.y / 2;
				if (Physics.CheckBox(checkPos, grid.minimumOpenAreaAroundMarkers * 0.5f))
				{
					return false;
				}
			}
				
			InstantiateMarker("Column " + rowIndex, markerPos, newColumn);
			return true;
		}
	}
}