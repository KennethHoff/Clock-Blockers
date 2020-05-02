using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using ClockBlockers.Utility;

using Sisus.OdinSerializer.Utilities;

using UnityEngine;


namespace ClockBlockers.MapData
{

	[ExecuteInEditMode]
	public class SurfaceColliderMarkerGenerator : MarkerGeneratorBase
	{
		private const float MaxDistanceCheck = 100f;
		[SerializeField]
		private float rayCastCeiling;
		
		[SerializeField]
		private float rayCastFloor;

		[SerializeField]
		private float maxDistanceBetweenMarkers = 10;
		
		private float RayCastHeight => rayCastCeiling - rayCastFloor;
		
		[SerializeField]
		private float boxCheckDistanceIncreasePerAttempt = 0.25f;

		[SerializeField]
		private int test_MarkerCreationAmount = 1;


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
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>();

			var startPos = new Vector3(grid.XStartPos, 0+0.05f, grid.ZStartPos);
			
			// startPos += grid.minimumOpenAreaAroundMarkers / 2;

			startPos.x += grid.minimumOpenAreaAroundMarkers.x / 2;
			startPos.y += grid.minimumOpenAreaAroundMarkers.y / 2;
			startPos.z -= grid.minimumOpenAreaAroundMarkers.z / 2;
			
			CreateMarker(startPos);
		}

		public override void ClearMarkers()
		{
			// while (gridTransform.childCount > 0)
			// {
				// DestroyImmediate(gridTransform.GetChild(0));
			// }

			foreach (PathfindingMarker marker in grid.markers.Where(marker => marker != null))
			{
				DestroyImmediate(marker.gameObject);
			}
		}

		public override void ResetAllMarkerGizmos()
		{
			// Logging.Log("Marker Reset is not yet implemented in SurfaceColliderMarkerGenerator.");
		}

		private void CreateMarker(Vector3 startPos)
		{
			// Start by creating a BoxCast at the start position [First iteration is the first-position on the map, given by PathfindingGrid - Upper-left], with start area equal to grid.MinimumOpenAreaAroundMarker

			// If it collides at one point, move the current marker position the other direction(If hit at top, go down, if hit at right, go left), and repeat the process.
			// If it collides at two points, but they're on the same side (Forward+Left / Forward+Right / Backward+Left / Backward+Right), do the same as if you collided with only one object. (If hit with Forward+left, go Backward+right)
			// If it collides at opposite corners (Backward + Top / Forward + Left), end this process.
			// Create a 'Marker' at this position, as well as a BoxCollider with the given area. (as well as a Gizmo for visualization)

			// Get the next start position << Haven't thought about how, just yet.

			Vector3 currPos = startPos;

			Vector3 currSize = grid.minimumOpenAreaAroundMarkers / 2;

			var collForward = false;
			var collLeft = false;
			var collBackward = false;
			var collRight = false;

			var iterations = 0;

			Logging.Log("Start position: " + currPos + " | Start size: " + currSize);

			FindMarkerSize(ref currPos, ref currSize, ref collForward, ref collLeft, ref collBackward, ref collRight, ref iterations);
			
			InstantiateMarker("Surface Collider Marker", currPos, currSize*2);

			if (gridTransform.childCount < test_MarkerCreationAmount)
			{
				var nextMarkerStartPos = new Vector3(currPos.x + currSize.x, currPos.y, currPos.z + currSize.z);
				CreateMarker(nextMarkerStartPos);
			}
		}

		private void FindMarkerSize(ref Vector3 currPos, ref Vector3 currSize, ref bool collForward, ref bool collLeft, ref bool collBackward, ref bool collRight, ref int iterations)
		{
			while (true)
			{

				if (iterations++ >= 1000)
				{
					Logging.Log("Too many iterations. Probably a logic error. Stopping");
					break;
				}


				if (currSize.x > MaxDistanceCheck || currSize.z > MaxDistanceCheck)
				{
					Logging.Log("Searched too far. Stopping");
					break;
				}

				if (currSize.x > maxDistanceBetweenMarkers || currSize.z > maxDistanceBetweenMarkers)
				{
					Logging.Log("Marker is as big as it gets. Stopping");
				}

				var boxCastPos = new Vector3(currPos.x, currPos.y, currPos.z + currSize.z);
				
				var boxCastSize = new Vector3(currSize.x/2, currSize.y/2, currSize.z);

				RaycastHit[] hits = Physics.BoxCastAll(boxCastPos, boxCastSize, Vector3.back, Quaternion.identity, currSize.z, ~grid.nonCollidingLayer);
				int size = hits.Length;


				// Collider[] colliders = Physics.OverlapBox(currPos, currSize, Quaternion.identity, ~grid.nonCollidingLayer);
				// int size = colliders.Length;

				if (size == 0)
				{
					Logging.Log("Current position: " + currPos + " | Current size: " + currSize + " || " + (collForward ? " Colliding Forward. " : "") + (collBackward ? " Colliding Backward. " : "") +
					            (collRight ? " Colliding Right. " : "") + (collLeft ? " Colliding Left. " : ""));
				}
				else 
				{
					string collNames = "";
					// foreach (Collider coll in colliders)
					// {
					// 	SetCollidingDirections(ref currPos, coll, ref collForward, ref collLeft, ref collBackward, ref collRight);
					// 	if (collNames != "") collNames += ",";
					// 	collNames += " " + coll.name;
					// }
					
					foreach (RaycastHit coll in hits)
					{
						Logging.Log("Coll point: " + coll.point);
						SetCollidingDirections(ref currPos, coll, ref collForward, ref collLeft, ref collBackward, ref collRight);
						if (collNames != "") collNames += ",";
						collNames += " " + coll.transform.name;
					}

					Logging.Log("Collided with" + collNames);

					Logging.Log("Current position: " + currPos + " | Current size: " + currSize + " || " + (collForward ? " Colliding Forward. " : "") + (collBackward ? " Colliding Backward. " : "") +
					            (collRight ? " Colliding Right. " : "") + (collLeft ? " Colliding Left. " : ""));

					if (collForward && collBackward)
					{
						Logging.Log("Colliding forward and backwards.. Stopping");
						break;
					}

					if (collLeft && collRight)
					{
						Logging.Log("Colliding Left and Right.. Stopping");
						break;
					}
					
					Logging.Log("Moving " + (collForward ? "Backwards" : " " ) + (collBackward ? "Forwards" : " " ) + (collLeft ? "Right" : " " ) + (collRight ? "Left" : " " ));

					if (collForward)
					{
						currPos.z -= boxCheckDistanceIncreasePerAttempt;
					}
					else if (collBackward)
					{
						currPos.z += boxCheckDistanceIncreasePerAttempt;
					}

					if (collRight)
					{
						currPos.x -= boxCheckDistanceIncreasePerAttempt;
					}
					else if (collLeft)
					{
						currPos.x += boxCheckDistanceIncreasePerAttempt;
					}
				}
				currSize.x += boxCheckDistanceIncreasePerAttempt;
				currSize.z += boxCheckDistanceIncreasePerAttempt;
				
			}
		}



		private void InstantiateMarker(string markerName, Vector3 markerPos, Vector3 colliderSize)
		{
			var newMarker = PathfindingMarker.CreateInstance(markerName, ref grid);
			
			newMarker.gameObject.layer = LayerInt;

			newMarker.transform.position = markerPos;
			newMarker.transform.SetParent(gridTransform);

			var newMarkerCollider = newMarker.gameObject.AddComponent<BoxCollider>();
			newMarkerCollider.size = colliderSize;
			newMarkerCollider.isTrigger = true;
		
			grid.markers.Add(newMarker);
		}
		
		private static void SetCollidingDirections(ref Vector3 currMarkerPos, RaycastHit hit, ref bool collForward, ref bool collLeft, ref bool collBackward, ref bool collRight)
		{

			var floatLeniency = 0.1f;
			// TODO: This seems backwards?

			Vector3 colliderPos = hit.point;

			if (colliderPos.z - currMarkerPos.z > floatLeniency)
			{
				Logging.Log(hit.transform.gameObject.name + " Collided backward at position: " + colliderPos);
				collBackward = true;
			}
			else
			{
				Logging.Log(hit.transform.gameObject.name + " Collided forward at position: " + colliderPos);
				collForward = true;
			}

			if (colliderPos.x - currMarkerPos.x > floatLeniency)
			{
				Logging.Log(hit.transform.gameObject.name + " Collided right at position: " + colliderPos);
				collRight = true;
			}
			else
			{
				Logging.Log(hit.transform.gameObject.name + " Collided left at position: " + colliderPos);
				collLeft = true;
			}
		}
		
		private static void SetCollidingDirections(ref Vector3 currMarkerPos, Collider collider, ref bool collForward, ref bool collLeft, ref bool collBackward, ref bool collRight)
		{
			// TODO: This seems backwards?

			Vector3 colliderPos = collider.ClosestPoint(currMarkerPos);

			if (colliderPos.z > currMarkerPos.z)
			{
				Logging.Log(collider.gameObject.name + " Collided backward");
				collBackward = true;
			}
			else
			{
				Logging.Log(collider.gameObject.name + " Collided forward");
				collForward = true;
			}

			if (colliderPos.x > currMarkerPos.x)
			{
				Logging.Log(collider.gameObject.name + " Collided right");
				collRight = true;
			}
			else
			{
				Logging.Log(collider.gameObject.name + " Collided left");
				collLeft = true;
			}
		}


		// private void CreateAMarkerOnEachCollision(int j, float xPos, Transform newRow, RaycastHit[] allCollisions, int i, float zPos, ref bool createdAtLeastOne)
		// {
		// 	RaycastHit hit = allCollisions[i];
		//
		// 	var markerPos = new Vector3(xPos, hit.point.y.Round(3), zPos);
		//
		// 	if (!grid.createMarkerNearOrInsideCollisions)
		// 	{
		// 		// If you're only colliding with yourself, then that's fine. Otherwise, create nothing 
		// 		
		// 		var newPos = new Vector3(markerPos.x, markerPos.y + grid.minimumOpenAreaAroundMarkers.y / 2, markerPos.z);
		//
		// 		Collider[] overlappingColliders = Physics.OverlapBox(newPos, grid.minimumOpenAreaAroundMarkers / 2, Quaternion.identity, ~grid.nonCollidingLayer);
		// 		if (!(overlappingColliders.Length == 1 && (overlappingColliders[0] = hit.collider))) return;
		// 	}
		//
		// 	string markerName = "Column " + j + (i > 0 ? "(" + i + ")" : "");
		// 	
		// 	markerPos.y += grid.creationHeightAboveFloor;
		//
		// 	// var markerSize = CheckPossibleArea(markerPos, out Vector3 dimensions);
		// 	
		// 	InstantiateMarker(markerName, markerSize, newRow, dimensions);
		// 	createdAtLeastOne = true;
		// }
		//
		// /// <summary>
		// /// Returns the center position around collisions
		// /// </summary>
		// /// <param name="markerPos"></param>
		// /// <param name="dimensions">Dimension of the collider, relative to the center position</param>
		// /// <returns></returns>
		// private Vector3 CheckPossibleArea(Vector3 markerPos, out Vector3 dimensions)
		// {
		// 	bool leftHit = Physics.Raycast(markerPos, Vector3.left, out RaycastHit leftRay, maxDistanceBetweenMarkers, ~grid.nonCollidingLayer);
		// 	bool rightHit = Physics.Raycast(markerPos, Vector3.right, out RaycastHit rightRay, maxDistanceBetweenMarkers, ~grid.nonCollidingLayer);
		// 	bool forwardHit = Physics.Raycast(markerPos, Vector3.forward, out RaycastHit forwardRay, maxDistanceBetweenMarkers, ~grid.nonCollidingLayer);
		// 	bool backwardHit = Physics.Raycast(markerPos, Vector3.back, out RaycastHit backwardRay, maxDistanceBetweenMarkers, ~grid.nonCollidingLayer);
		//
		// 	float leftDistance = leftHit ? leftRay.distance : maxDistanceBetweenMarkers;
		// 	float rightDistance = rightHit ? rightRay.distance : maxDistanceBetweenMarkers;
		// 	float forwardDistance = forwardHit ? forwardRay.distance : maxDistanceBetweenMarkers;
		// 	float backwardDistance = backwardHit ? backwardRay.distance : maxDistanceBetweenMarkers;
		//
		// 	Vector3 topLeft = new Vector3(markerPos.x - leftDistance, markerPos.y, markerPos.z + forwardDistance);
		// 	
		// 	Vector3 topRight = new Vector3(markerPos.x + rightDistance, markerPos.y, markerPos.z + forwardDistance);
		//
		// 	Vector3 bottomLeft = new Vector3(markerPos.x - leftDistance, markerPos.y, markerPos.z - backwardDistance);
		// 	
		// 	Vector3 bottomRight = new Vector3(markerPos.x + rightDistance, markerPos.y, markerPos.z - forwardDistance);
		//
		// 	dimensions = new Vector3(leftDistance + rightDistance, 0.1f, forwardDistance + backwardDistance);
		//
		// 	Vector3 center = Vector3.zero;
		//
		// 	center += topLeft;
		// 	center += topRight;
		// 	center += bottomLeft;
		// 	center += bottomRight;
		//
		// 	center /= 4;
		//
		// 	return center;
		// }


		

	}
}