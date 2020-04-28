using System.Collections.Generic;

using ClockBlockers.MapData.Grid;
using ClockBlockers.MapData.Marker;

using UnityEngine;


namespace ClockBlockers.MapData
{
	public class PathfindingMarkerRow : MonoBehaviour
	{
		// public List<PathfindingMarker> markers;

		// [HideInInspector]
		// public PathfindingGrid grid;

		// public void GenerateMarkerAdjacencies()
		// {
			// foreach (PathfindingMarker marker in markers)
			// {
				// marker.GetAdjacentMarkersFromGrid();
			// }
		// }

		// public void ResetAllMarkerGizmos()
		// {
			// foreach (PathfindingMarker marker in markers)
			// {
				// marker.scale = grid.nodeScale;
				// marker.PickAColor();
			// }
		// }

		public static PathfindingMarkerRow CreateInstance(string name, PathfindingGrid newGrid)
		{
			PathfindingMarkerRow newRow = new GameObject(name).AddComponent<PathfindingMarkerRow>();
			
			// newRow.markers = new List<PathfindingMarker>(newGrid.MaximumNumberOfColumns);
			// newRow.grid = newGrid;

			return newRow;
		}
	}
}