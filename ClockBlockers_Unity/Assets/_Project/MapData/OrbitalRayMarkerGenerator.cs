using System;
using System.Collections.Generic;

using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData
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
			
			var originPos = new Vector3(xPos, rayCastCeiling, zPos);
			Vector3 direction = Vector3.down;
			RaycastHit[] allCollisions = Physics.RaycastAll(originPos, direction, MarkerCreationDistanceCheck, grid.pathfindingLayer);

			int numberOfCollisions = allCollisions.Length;
			if (numberOfCollisions == 0) return false;

			var createdAtLeastOne = false;

			for (var i = 0; i < numberOfCollisions; i++)
			{
				CreateAMarkerOnEachCollision(j, xPos, newRow, allCollisions, i, zPos, ref createdAtLeastOne);
			}
			return createdAtLeastOne;
		}

		private void CreateAMarkerOnEachCollision(int j, float xPos, Transform newRow, RaycastHit[] allCollisions, int i, float zPos, ref bool createdAtLeastOne)
		{
			RaycastHit hit = allCollisions[i];

			var markerPos = new Vector3(xPos, hit.point.y.Round(3), zPos);

			if (!grid.createMarkerNearOrInsideCollisions)
			{
				var newPos = new Vector3(markerPos.x, markerPos.y + grid.minimumOpenAreaAroundMarkers.y / 2, markerPos.z);
				// var newPos = new Vector3(hit.point.x, hit.point.y + grid.minimumOpenAreaAroundMarkers.y / 2, hit.point.z);

				Collider[] overlappingColliders = Physics.OverlapBox(newPos, grid.minimumOpenAreaAroundMarkers / 2, Quaternion.identity, ~grid.nonCollidingLayer);
				if (!(overlappingColliders.Length == 1 && (overlappingColliders[0] = hit.collider))) return;
			}

			string markerName = "Column " + j + (i > 0 ? "(" + i + ")" : "");
			
			markerPos.y += grid.creationHeightAboveFloor;
			InstantiateMarker(markerName, markerPos, newRow);
			createdAtLeastOne = true;
		}

		private void InstantiateMarker(string markerName, Vector3 markerPos, Transform parent)
		{
			// PathfindingMarker newMarker = Instantiate(grid.markerPrefab, markerPos, Quaternion.identity, parent);
			var newMarker = PathfindingMarker.CreateInstance(markerName, ref markerPos, ref parent, ref grid.creationHeightAboveFloor);
			
			newMarker.Grid = grid;

			grid.markers.Add(newMarker);
		}
	}
}