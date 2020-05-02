using System.Collections.Generic;

using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData
{
	[ExecuteInEditMode]
	public class OrbitalRayMarkerGenerator : IntervalMarkerGeneratorBase
	{
		[SerializeField]
		private float rayCastCeiling = 20f;
		
		[SerializeField]
		private float rayCastFloor = -0.5f;


		// I want to only allow integer inputs, but I want to get fractions when dividing

		private float RayCastHeight => rayCastCeiling - rayCastFloor;
		private float MarkerCreationDistanceCheck => rayCastCeiling - rayCastFloor;

		private void OnDrawGizmos()
		{
			if (!showAffectedArea) return;
			Gizmos.color = Color.red;
			
			var wireCubePos = new Vector3(0, (rayCastFloor + rayCastCeiling) / 2, 0);
			
			var wireCubeSize = new Vector3(grid.XLength, RayCastHeight, grid.ZLength);
			
			Gizmos.DrawWireCube(wireCubePos, wireCubeSize);
		}

		private void OnValidate()
		{
			if (rayCastCeiling < rayCastFloor) rayCastCeiling = rayCastFloor;
			if (rayCastFloor > rayCastCeiling) rayCastFloor = rayCastCeiling;
		}

		public override void GenerateAllMarkers()
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

		public override void ClearMarkers()
		{
			foreach (PathfindingMarker marker in grid.markers)
			{
				if (marker == null) continue;
				DestroyImmediate(marker.transform.parent.gameObject);
			}
			grid.markers.Clear();

		}

		private void CreateMarkerColumn(int i)
		{
			float xPos = MarkerSizeAdjustedXStartPos + (xDistanceBetweenMarkers * i);

			Transform newColumn = new GameObject("Column " + i).transform;

			newColumn.position = new Vector3(xPos, 0, 0);

			newColumn.SetParent(gridTransform);

			var createdNothing = true;

			for (var j = 0; j <= NumberOfRows; j++)
			{
				if (CreateMarker(j, xPos, newColumn)) createdNothing = false;
			}

			if (!createdNothing) return;
			DestroyImmediate(newColumn.gameObject);
			Logging.Log("Did not create any Markers on column " + i);
		}

		private bool CreateMarker(int j, float xPos, Transform newColumn)
		{
			float zPos = MarkerSizeAdjustedZStartPos - (zDistanceBetweenMarkers * j);
			
			var originPos = new Vector3(xPos, rayCastCeiling, zPos);
			Vector3 direction = Vector3.down;
			RaycastHit[] allCollisions = Physics.RaycastAll(originPos, direction, MarkerCreationDistanceCheck, grid.pathfindingLayer);

			int numberOfCollisions = allCollisions.Length;
			if (numberOfCollisions == 0) return false;

			var createdAtLeastOne = false;

			for (var i = 0; i < numberOfCollisions; i++)
			{
				CreateAMarkerOnEachCollision(j, xPos, newColumn, allCollisions, i, zPos, ref createdAtLeastOne);
			}

			if (!createdAtLeastOne)
			{
				Logging.Log("Did not create any Markers on column " + j);
			}
			return createdAtLeastOne;
		}

		private void CreateAMarkerOnEachCollision(int j, float xPos, Transform newRow, RaycastHit[] allCollisions, int i, float zPos, ref bool createdAtLeastOne)
		{
			RaycastHit hit = allCollisions[i];

			var markerPos = new Vector3(xPos, hit.point.y.Round(3), zPos);

			if (!grid.createMarkerNearOrInsideCollisions)
			{
				// If you're only colliding with yourself, then that's fine. Otherwise, create nothing 
				
				var newPos = new Vector3(markerPos.x, markerPos.y + grid.minimumOpenAreaAroundMarkers.y / 2, markerPos.z);

				Collider[] overlappingColliders = Physics.OverlapBox(newPos, grid.minimumOpenAreaAroundMarkers / 2, Quaternion.identity, ~grid.nonCollidingLayer);
				if (!(overlappingColliders.Length == 1 && (overlappingColliders[0] = hit.collider))) return;
			}

			string markerName = "Row " + j + (i > 0 ? "(" + i + ")" : "");
			
			markerPos.y += grid.creationHeightAboveFloor;

			InstantiateMarker(markerName, markerPos, newRow);
			createdAtLeastOne = true;
		}

		private void InstantiateMarker(string markerName, Vector3 markerPos, Transform parent)
		{
			// PathfindingMarker newMarker = Instantiate(grid.markerPrefab, markerPos, Quaternion.identity, parent);
			var newMarker = PathfindingMarker.CreateInstance(markerName, ref grid, ref markerPos, ref parent, ref grid.creationHeightAboveFloor);
			
			newMarker.Grid = grid;
			newMarker.gameObject.layer = LayerInt;
			
			grid.markers.Add(newMarker);
		}
	}
}