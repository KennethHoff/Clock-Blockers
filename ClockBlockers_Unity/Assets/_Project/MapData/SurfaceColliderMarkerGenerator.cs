using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using UnityEngine;

namespace ClockBlockers.MapData
{

	[ExecuteInEditMode]
	public class SurfaceColliderMarkerGenerator : MarkerGeneratorBase
	{
		
		private const float MaxDistanceCheck = 100f;
		
		[Space(10)]
		[SerializeField]
		private float maxDistanceBetweenMarkers = 10;
		
		[SerializeField]
		private float boxCheckDistanceIncreasePerAttempt = 0.25f;

		[SerializeField]
		private int maximumCreatedMarkers = 1;

		private void OnDrawGizmos()
		{
			if (!showAffectedArea) return;
			Gizmos.color = Color.red;
			
		}

		public override void GenerateAllMarkers()
		{
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>();

			var startPos = new Vector3(grid.XStartPos, 0, grid.ZStartPos);
			
			// startPos += grid.minimumOpenAreaAroundMarkers / 2;

			startPos.x += grid.minimumOpenAreaAroundMarkers.x;
			startPos.y += grid.minimumOpenAreaAroundMarkers.y / 2 + 0.1f;
			startPos.z -= grid.minimumOpenAreaAroundMarkers.z;
			
			CreateMarker(startPos);
		}

		public override void ClearMarkers()
		{
			foreach (PathfindingMarker marker in grid.markers.Where(marker => marker != null))
			{
				DestroyImmediate(marker.gameObject);
			}
			
			grid.markers.Clear();
		}

		public override void GenerateMarkerAdjacencies()
		{
			Logging.Log("Surface Collider Marker Generator has not implemented GenerateMarkerAdjacencies");
		}

		private void CreateMarker(Vector3 startPos)
		{
			Vector3[] markerPositionAndSize = GetMarkerPositionAndSize(startPos);

			Vector3 markerPos = markerPositionAndSize[0];
			Vector3 markerSize = markerPositionAndSize[1] * 2;

			InstantiateMarker("Surface Collider Marker", markerPos, markerSize);

			
			float nextMarkerXStartPos = markerPos.x + (markerSize.x / 2 - 0.5f);
			float nextMarkerYStartPos = markerPos.y;
			float nextMarkerZStartPos = markerPos.z + (markerSize.z/2-0.5f);
			
			var tooFarOnX = false;
			var tooFarOnZ = false;
			
			if (markerPos.x + markerSize.x/2 >= grid.XEndPos)
			{
				Logging.Log("Over the edge on the X-axis.");
				tooFarOnX = true;
			}

			if (markerPos.z + markerSize.z/2 <= grid.ZEndPos)
			{
				Logging.Log("Over the edge on the Z-axis.");
				tooFarOnZ = true;
			}

			if (tooFarOnX && tooFarOnZ)
			{
				Logging.Log("Too far on both X and Z. Stopped creating markers!");
				return;
			}

			if (tooFarOnX)
			{
				nextMarkerXStartPos = grid.XStartPos + grid.minimumOpenAreaAroundMarkers.x;
				nextMarkerZStartPos -= markerSize.z/2;
			}

			if (gridTransform.childCount >= maximumCreatedMarkers)
			{
				Logging.Log("Created enough markers. Stopped creating markers");
				return;
			}
			
			// Because all objects should be on a 'scale increment' of 1, removing 0.5f should then cause it to not be inside the object
				
			var nextMarkerStartPos = new Vector3(nextMarkerXStartPos, nextMarkerYStartPos, nextMarkerZStartPos);
			CreateMarker(nextMarkerStartPos);
		}

		private Vector3[] GetMarkerPositionAndSize(Vector3 startPos)
		{
			// Start by creating a BoxCast at the start position [First iteration is the first-position on the map, given by PathfindingGrid - Upper-left], with start area equal to grid.MinimumOpenAreaAroundMarker

			// If it collides at one point, move the current marker position the other direction(If hit at top, go down, if hit at right, go left), and repeat the process.
			// If it collides at two points, but they're on the same side (Forward+Left / Forward+Right / Backward+Left / Backward+Right), do the same as if you collided with only one object. (If hit with Forward+left, go Backward+right)
			// If it collides at opposite corners (Backward + Top / Forward + Left), end this process.
			// Create a 'Marker' at this position, as well as a BoxCollider with the given area. (as well as a Gizmo for visualization)

			// Get the next start position << Haven't thought about how, just yet.
			
			// TODO: If it hits a wall, unless that position is outside the edges of the map, start the next marker on the other side of that wall.

			Vector3 currPos = startPos;

			Vector3 currSize = grid.minimumOpenAreaAroundMarkers / 2;

			var collForward = false;
			var collLeft = false;
			var collBackward = false;
			var collRight = false;

			var iterations = 0;

			Logging.Log("Start position: " + currPos + " | Start size: " + currSize);

			FindMarkerSize(ref currPos, ref currSize, ref collForward, ref collLeft, ref collBackward, ref collRight, ref iterations);

			return new[] {currPos, currSize};
		}

		private void FindMarkerSize(ref Vector3 currPos, ref Vector3 currSize, ref bool collForward, ref bool collLeft, ref bool collBackward, ref bool collRight, ref int iterations)
		{
			while (true)
			{

				if (iterations++ >= 1000)
				{
					Logging.Log("Too many iterations. Probably a logic error. Stopping");
					return;
				}
				
				if (currSize.x > MaxDistanceCheck || currSize.z > MaxDistanceCheck)
				{
					Logging.Log("Searched too far. Stopping");
					return;
				}

				if (currSize.x > maxDistanceBetweenMarkers || currSize.z > maxDistanceBetweenMarkers)
				{
					Logging.Log("Marker is as big as it gets. Stopping");
					return;
				}

				Collider[] colliders = Physics.OverlapBox(currPos, currSize, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);
				int size = colliders.Length;

				if (size == 0)
				{
					Logging.Log("Iteration " + iterations + " | Current position: " + currPos + " | Current size: " + currSize + " || " + (collForward ? " Colliding Forward. " : "") + (collBackward ? " Colliding Backward. " : "") +
					            (collRight ? " Colliding Right. " : "") + (collLeft ? " Colliding Left. " : ""));
				}
				else 
				{
					string collNames = "";
					foreach (Collider coll in colliders)
					{
						SetCollidingDirections(ref currPos, coll, ref collForward, ref collLeft, ref collBackward, ref collRight);
						if (collNames != "") collNames += ",";
						collNames += " " + coll.name;
					}
					
					Logging.Log("Collided with" + collNames);

					Logging.Log("Iteration " + iterations + " | Current position: " + currPos + " | Current size: " + currSize + " || " + (collForward ? " Colliding Forward. " : "") + (collBackward ? " Colliding Backward. " : "") +
					            (collRight ? " Colliding Right. " : "") + (collLeft ? " Colliding Left. " : ""));

					if (collForward && collBackward)
					{
						Logging.Log("Colliding forward and backwards.. Stopping");
						return;
					}

					if (collLeft && collRight)
					{
						Logging.Log("Colliding Left and Right.. Stopping");
						return;
					}
					
					if (collLeft || collRight || collBackward || collForward)
					{
						Logging.Log("Moving " + (collForward ? "Backwards" : " " ) + (collBackward ? "Forwards" : " " ) + (collLeft ? "Right" : " " ) + (collRight ? "Left" : " " ));
					}
					
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
			// var newMarker = PathfindingMarker.CreateInstance(markerName, ref grid);
			var newMarker = PathfindingMarker.CreateInstance(markerName + " " + grid.markers.Count, ref grid);
			
			newMarker.transform.position = markerPos;
			newMarker.transform.SetParent(gridTransform);

			var newMarkerCollider = newMarker.gameObject.AddComponent<BoxCollider>();
			newMarkerCollider.size = colliderSize;
			// newMarkerCollider.isTrigger = true;

			Logging.Log("");
			Logging.Log("");
			Logging.Log("");
			Logging.Log("");
			Logging.Log("INSTANTIATED MARKER #" + grid.markers.Count);
			Logging.Log("");
			Logging.Log("");
			Logging.Log("");
			Logging.Log("");
			
			grid.markers.Add(newMarker);
		}


		private static void SetCollidingDirections(ref Vector3 currMarkerPos, Collider collider, ref bool collForward, ref bool collLeft, ref bool collBackward, ref bool collRight)
		{
			Vector3 colliderPos = collider.ClosestPointOnBounds(currMarkerPos);
			SetCollidingDirections(ref currMarkerPos, colliderPos, ref collForward, ref collLeft, ref collBackward, ref collRight, collider.gameObject);
		}

		private static void SetCollidingDirections(ref Vector3 currMarkerPos, Vector3 hitPos, ref bool collForward, ref bool collLeft, ref bool collBackward, ref bool collRight, GameObject gObjHit)
		{
			var floatLeniency = 0.25f;

			// TODO: This seems backwards?

			float zDist = hitPos.z - currMarkerPos.z;

			float xDist = hitPos.x - currMarkerPos.x;

			if (Math.Abs(zDist) > floatLeniency)
			{
				if (Math.Sign(zDist) == 1)
				{
					Logging.Log(gObjHit.name + " Collided forward at position: " + hitPos);
					collForward = true;
				}
				else
				{
					Logging.Log(gObjHit.name + " Collided backward at position: " + hitPos);
					collBackward = true;
				}
			}

			if (Math.Abs(xDist) > floatLeniency)
			{
				if (Math.Sign(xDist) == 1)
				{
					Logging.Log(gObjHit.name + " Collided right at position: " + hitPos);
					collRight = true;
				}
				else
				{
					Logging.Log(gObjHit.name + " Collided left at position: " + hitPos);
					collLeft = true;
				}
			}
		}
	}
}