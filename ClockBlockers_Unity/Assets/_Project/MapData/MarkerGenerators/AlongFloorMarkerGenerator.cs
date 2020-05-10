using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	[ExecuteInEditMode]
	[BurstCompile]

	public class AlongFloorMarkerGenerator : IntervalMarkerGeneratorBase
	{
		protected override int CreateMarker(float xPos, int rowIndex, Transform newColumn, int columnIndex)
		{
			float zPos = MarkerSizeAdjustedZStartPos - (distanceBetweenCreatedMarkers * rowIndex); 
			
			var markerPos = new Vector3(xPos, creationHeightAboveFloor, zPos);

			if (!createMarkerNearOrInsideCollisions)
			{
				Vector3 checkPos = markerPos;
				checkPos.y += grid.minimumOpenAreaAroundMarkers.y / 2;
				if (Physics.CheckBox(checkPos, grid.minimumOpenAreaAroundMarkers * 0.5f))
				{
					return 0;
				}
			}
				
			InstantiateMarker("Column " + rowIndex, markerPos, newColumn);
			return 1;
		}
	}
}