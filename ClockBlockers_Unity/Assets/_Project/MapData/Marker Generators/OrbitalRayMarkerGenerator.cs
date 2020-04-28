using System.Collections.Generic;

using ClockBlockers.MapData.Grid;
using ClockBlockers.MapData.Marker;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData.Marker_Generators
{
	[ExecuteInEditMode]
	public class OrbitalRayMarkerGenerator : MonoBehaviour, IMarkerGenerator
	{
		
		[SerializeField]
		private bool showAffectedArea;
		[SerializeField]
		private float rayCastCeiling;
		
		[SerializeField]
		private float rayCastFloor;

		private float RayCastHeight => rayCastCeiling - rayCastFloor;
		private float MarkerCreationDistanceCheck => rayCastCeiling - rayCastFloor;
		
		private PathfindingGrid grid;
		private Transform gridTransform;

		private void Awake()
		{
			grid = GetComponent<PathfindingGrid>();
			gridTransform = grid.transform;
		}

		private void OnDrawGizmos()
		{
			if (!showAffectedArea) return;
			Gizmos.color = Color.red;
			
			var wireCubePos = new Vector3(grid.XStartPos + grid.XLength / 2, (rayCastFloor + rayCastCeiling) / 2, grid.ZStartPos + grid.ZLength / 2);
			var wireCubeSize = new Vector3(grid.XLength, RayCastHeight, grid.ZLength);
			Gizmos.DrawWireCube(wireCubePos, wireCubeSize);
		}

		private void OnValidate()
		{
			if (rayCastCeiling < rayCastFloor) rayCastCeiling = rayCastFloor;
			if (rayCastFloor > rayCastCeiling) rayCastFloor = rayCastCeiling;
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
			//
			// grid.markerRows.Add(newRow);
			//
			// newRow.grid = grid;
			//
			// newRow.markers = new List<PathfindingMarker>(grid.ExpectedNumberOfColumns);

			// PathfindingMarkerRow newRow = PathfindingMarkerRow.CreateInstance("Row " + i, grid);
			// Transform rowTransform = newRow.transform;

			
			Transform rowTransform = new GameObject("Row " + i).transform;
			

			rowTransform.position = new Vector3(xPos, 0, 0);

			rowTransform.SetParent(gridTransform);

			// var createdNothing = true;

			for (var j = 0; j <= grid.MaximumNumberOfRows; j++)
			{
				// if (CreateMarker(j, xPos, newRow)) createdNothing = false;
				CreateMarker(j, xPos, ref rowTransform);
			}
			// if (createdNothing) DestroyImmediate(newRow.gameObject);
		}

		private bool CreateMarker(int j, float xPos, ref Transform rowTransform)
		{
			float zPos = grid.ZStartPos + (grid.zDistanceBetweenMarkers * j);
			
			var originPos = new Vector3(xPos, rayCastCeiling, zPos);
			Vector3 direction = Vector3.down;
			RaycastHit[] allCollisions = Physics.RaycastAll(originPos, direction, MarkerCreationDistanceCheck, grid.pathfindingLayer);

			int numberOfCollisions = allCollisions.Length;
			if (numberOfCollisions == 0) return false;

			var createdAtLeastOne = false;

			for (var i = 0; i < numberOfCollisions; i++)
			{
				CreateAMarkerOnEachCollision(j, xPos, ref rowTransform, allCollisions, i, zPos, ref createdAtLeastOne);
			}
			return createdAtLeastOne;
		}

		private void CreateAMarkerOnEachCollision(int j, float xPos, ref Transform rowTransform, IReadOnlyList<RaycastHit> allCollisions, int i, float zPos, ref bool createdAtLeastOne)
		{
			RaycastHit hit = allCollisions[i];

			var markerPos = new Vector3(xPos, hit.point.y.Round(3), zPos);


			if (!grid.createMarkerNearOrInsideCollisions)
			{
				var newPos = new Vector3(markerPos.x, markerPos.y + grid.minimumOpenAreaAroundMarkers.y / 2, markerPos.z);

				Collider[] overlappingColliders = Physics.OverlapBox(newPos, grid.minimumOpenAreaAroundMarkers / 2, Quaternion.identity, ~grid.nonCollidingLayer);
				if (!(overlappingColliders.Length == 1 && (overlappingColliders[0] = hit.collider))) return;
			}

			string markerName = "Column " + j + (i > 0 ? "(" + i + ")" : "");

			var newMarker = PathfindingMarker.CreateInstance(markerName, ref markerPos, ref grid, ref rowTransform);
			
			grid.markers.Add(newMarker);

		}
	}
}