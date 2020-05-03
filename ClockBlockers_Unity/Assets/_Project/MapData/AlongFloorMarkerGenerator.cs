using ClockBlockers.Utility;

using UnityEngine;

namespace ClockBlockers.MapData
{
	[ExecuteInEditMode]
	public class AlongFloorMarkerGenerator : IntervalMarkerGeneratorBase
	{
		protected override bool CreateMarker(int j, float xPos, Transform newRow)
		{
			float zPos = MarkerSizeAdjustedZStartPos - (zDistanceBetweenMarkers * j); 
			
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
				
			InstantiateMarker("Column " + j, markerPos, ref newRow);
			return true;
		}
	}
}