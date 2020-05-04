using System.Collections.Generic;

using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	[ExecuteInEditMode]
	public class OrbitalRayMarkerGenerator : IntervalAutomatedMarkerGenerator
	{
		[SerializeField]
		private float rayCastCeiling = 20f;
		
		[SerializeField]
		private float rayCastFloor = -0.5f;
		
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

		protected override bool CreateMarker(int j, float xPos, Transform newColumn)
		{
			float zPos = MarkerSizeAdjustedZStartPos - (zDistanceBetweenMarkers * j);
			
			if (!CheckValidMarkerHeights(xPos, zPos, out RaycastHit[] allCollisions)) return false;

			bool createdAtLeastOne = CreateAMarkerOnEachValidHeight(allCollisions, j, xPos, newColumn, zPos);
			
			return createdAtLeastOne;
		}

		private bool CheckValidMarkerHeights(float xPos, float zPos, out RaycastHit[] allCollisions)
		{
			var rayOriginPos = new Vector3(xPos, rayCastCeiling, zPos);
			Vector3 rayDirection = Vector3.down;
			
			allCollisions = Physics.RaycastAll(rayOriginPos, rayDirection, MarkerCreationDistanceCheck);

			return allCollisions.Length != 0;
		}

		private bool CreateAMarkerOnEachValidHeight(IReadOnlyList<RaycastHit> allCollisions, int j, float xPos, Transform newRow, float zPos)
		{
			bool createdAtLeastOne = false;
			for (var i = 0; i < allCollisions.Count; i++)
			{
				RaycastHit hit = allCollisions[i];

				var markerPos = new Vector3(xPos, hit.point.y.Round(3), zPos);
				// var markerPos = new Vector3(xPos, hit.point.y.Round(3) + (grid.minimumOpenAreaAroundMarkers.y/2), zPos);

				if (!createMarkerNearOrInsideCollisions)
				{
					// If you're only colliding with yourself, then that's fine. Otherwise, create nothing 

					var newPos = new Vector3(markerPos.x, markerPos.y + grid.minimumOpenAreaAroundMarkers.y / 2, markerPos.z);

					// newPos.y += 0.1f;

					var halfExtents = grid.minimumOpenAreaAroundMarkers / 2;
					halfExtents.x *= 0.975f;
					halfExtents.z *= 0.975f;

					Collider[] overlappingColliders = Physics.OverlapBox(newPos, halfExtents, Quaternion.identity);
					if (!(overlappingColliders.Length == 1 && (overlappingColliders[0] = hit.collider))) continue;
				}

				string markerName = "Row " + j + (i > 0 ? "(" + i + ")" : "");

				// markerPos.y += creationHeightAboveFloor + (grid.minimumOpenAreaAroundMarkers.y / 2);

				markerPos.y += creationHeightAboveFloor;

				InstantiateMarker(markerName, markerPos, ref newRow);
				createdAtLeastOne = true;
			}
			return createdAtLeastOne;
		}
	}
}