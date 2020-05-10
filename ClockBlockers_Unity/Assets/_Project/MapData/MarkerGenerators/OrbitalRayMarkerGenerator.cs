using System.Collections.Generic;

using ClockBlockers.Characters;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	[BurstCompile]

	[ExecuteInEditMode]
	public class OrbitalRayMarkerGenerator : IntervalMarkerGeneratorBase
	{
		private void OnDrawGizmos()
		{
			if (!showAffectedArea) return;
			
			Gizmos.color = Color.red;
			
			var wireCubePos = new Vector3(0, (grid.YStartPos + grid.YEndPos) / 2, 0);

			var wireCubeSize = new Vector3(grid.XLength, grid.MapHeight, grid.ZLength);
			
			Gizmos.DrawWireCube(wireCubePos, wireCubeSize);
		}

		protected override int CreateMarker(float xPos, int rowIndex, Transform newColumn, int columnIndex)
		{
			float zPos = MarkerSizeAdjustedZStartPos - (distanceBetweenCreatedMarkers * rowIndex);
			
			if (!CheckValidMarkerHeights(xPos, zPos, out RaycastHit[] allCollisions)) return 0;

			int markersCreated = CreateAMarkerOnEachValidHeight(allCollisions, xPos, zPos, rowIndex, newColumn, columnIndex);
			
			return markersCreated;
		}

		private bool CheckValidMarkerHeights(float xPos, float zPos, out RaycastHit[] allCollisions)
		{
			var rayOriginPos = new Vector3(xPos, grid.YEndPos, zPos);

			var ray = new Ray(rayOriginPos, Vector3.down);

			allCollisions = RayCaster.CastRayAll(ray, grid.MapHeight);

			return allCollisions.Length != 0;
		}

		private int CreateAMarkerOnEachValidHeight(IReadOnlyList<RaycastHit> allCollisions, float xPos, float zPos, int rowIndex, Transform parentColumn, int columnIndex)
		{
			var markersCreated = new List<PathfindingMarker>();
			int collCount = allCollisions.Count;
			for (var i = 0; i < collCount; i++)
			{
				// I want to get the upper-most hit first
				RaycastHit hit = allCollisions[collCount-1-i];

				var markerPos = new Vector3(xPos, hit.point.y.Round(3), zPos);

				if (!createMarkerNearOrInsideCollisions)
				{
					// If you're only colliding with yourself, then that's fine. Otherwise, create nothing 

					var newPos = new Vector3(markerPos.x, markerPos.y + grid.minimumOpenAreaAroundMarkers.y / 2, markerPos.z);

					// newPos.y += 0.1f;

					Vector3 halfExtents = grid.minimumOpenAreaAroundMarkers / 2;
					halfExtents.x *= 0.975f;
					halfExtents.z *= 0.975f;

					Collider[] overlappingColliders = Physics.OverlapBox(newPos, halfExtents, Quaternion.identity);
					if (!(overlappingColliders.Length == 1 && (overlappingColliders[0] = hit.collider))) continue;
				}

				string markerName = $"Column {columnIndex} Row {rowIndex}";

				// markerPos.y += creationHeightAboveFloor + (grid.minimumOpenAreaAroundMarkers.y / 2);

				markerPos.y += creationHeightAboveFloor;

				PathfindingMarker newMarker = InstantiateMarker(markerName, markerPos, parentColumn);
				
				markersCreated.Add(newMarker);
			}

			int markersCreatedCount = markersCreated.Count;
			
			// ReSharper disable once InvertIf
			if (markersCreatedCount > 1)
			{
				for (var i = 0; i < markersCreatedCount; i++)
				{
					PathfindingMarker marker = markersCreated[i];
					marker.name += $"({i})";
				}
			}

			return markersCreatedCount;
		}
	}
}