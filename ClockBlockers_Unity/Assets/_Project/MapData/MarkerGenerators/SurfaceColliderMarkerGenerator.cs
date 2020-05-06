using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{

	[ExecuteInEditMode]
	public class SurfaceColliderMarkerGenerator : AutomatedMarkerGenerator
	{
		
		private const float MaxDistanceCheck = 100f;
		
		[Space(10)]
		[SerializeField]
		private float maxMarkerSize = 10;
		
		[SerializeField]
		private float boxCheckDistanceIncreasePerAttempt = 0.25f;

		[SerializeField]
		private int maximumCreatedMarkers = 1;

		private void OnDrawGizmos()
		{
			if (!showAffectedArea) return;
			Gizmos.color = Color.red;
			
		}

		private void OnValidate()
		{
			if (maximumCreatedMarkers < 0) maximumCreatedMarkers = 0;
		}

		public override void GenerateAllMarkers()
		{
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>();

			var startPos = new Vector3(grid.XStartPos, 0, grid.ZStartPos);
			
			// startPos += grid.minimumOpenAreaAroundMarkers / 2;

			startPos.x += grid.minimumOpenAreaAroundMarkers.x;
			startPos.y += grid.minimumOpenAreaAroundMarkers.y / 2 + 0.1f;
			startPos.z -= grid.minimumOpenAreaAroundMarkers.z;
			
			IterativelyCreateMarkers(startPos);
		}

		public override void ClearMarkers()
		{
			foreach (PathfindingMarker marker in grid.markers.Where(marker => marker != null))
			{
				DestroyImmediate(marker.gameObject);
			}

			grid.ClearMarkerList();
			grid.ClearDictionary();
		}

		public override void GenerateMarkerAdjacencies()
		{
			Logging.Log("Surface Collider Marker Generator has not implemented GenerateMarkerAdjacencies");
		}

		private void IterativelyCreateMarkers(Vector3 startPos)
		{
			while (true)
			{
				Vector3[] markerPositionAndSize = GetMarkerPositionAndSize(startPos);

				Vector3 markerPos = markerPositionAndSize[0];
				Vector3 markerSize = markerPositionAndSize[1] * 2;

				InstantiateMarker("Surface Collider Marker", markerPos, markerSize);

				float nextMarkerXStartPos = markerPos.x + (markerSize.x / 2) + 0.5f;
				float nextMarkerYStartPos = markerPos.y;

				float nextMarkerZStartPos = markerPos.z;

				var tooFarOnX = false;
				var tooFarOnZ = false;

				if (markerPos.x + markerSize.x / 2 >= grid.XEndPos)
				{
					Logging.Log("Over the edge on the X-axis.");
					tooFarOnX = true;
				}

				if (markerPos.z + markerSize.z / 2 <= grid.ZEndPos)
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
					nextMarkerZStartPos -= markerSize.z;
				}

				if (gridTransform.childCount >= maximumCreatedMarkers)
				{
					Logging.Log("Created enough markers. Stopped creating markers");
					return;
				}

				var nextMarkerStartPos = new Vector3(nextMarkerXStartPos, nextMarkerYStartPos, nextMarkerZStartPos);
				startPos = nextMarkerStartPos;
			}
		}

		private Vector3[] GetMarkerPositionAndSize(Vector3 startPos)
		{
			// Start by creating a BoxCast at the start position [First iteration is the first-position on the map, given by PathfindingGrid - Upper-left], with start area equal to grid.MinimumOpenAreaAroundMarker

			// If it collides at one point, move the current marker position the other direction(If hit at top, go down, if hit at right, go left), and repeat the process.
			// If it collides at two points, but they're on the same side (Forward+Left / Forward+Right / Back+Left / Back+Right), do the same as if you collided with only one object. (If hit with Forward+left, go Back+right)
			// If it collides at opposite corners (Back + Top / Forward + Left), end this process.
			// Create a 'Marker' at this position, as well as a BoxCollider with the given area. (as well as a Gizmo for visualization)

			// Get the next start position << Haven't thought about how, just yet.
			
			// TODO: If it hits a wall, unless that position is outside the edges of the map, start the next marker on the other side of that wall.

			Vector3 currPos = startPos;

			Vector3 currSize = grid.minimumOpenAreaAroundMarkers / 2;
			
			Logging.Log("Start position: " + currPos + " | Start size: " + currSize);

			FindMarkerPosAndSize(ref currPos, ref currSize);

			return new[] {currPos, currSize};
		}

		private void FindMarkerPosAndSize(ref Vector3 currPos, ref Vector3 currSize)
		{
			var iterations = 0;

			while (true)
			{
				Logging.Log($"Iteration {iterations} | Current position: {currPos.ToString("F4")} | Current Size: {currSize.ToString("F4")}");

				#region Guard Clauses

				if (iterations++ >= 250)
				{
					Logging.Log("Too many iterations. Probably a logic error. Stopping");
					return;
				}

				if (currSize.x >= MaxDistanceCheck || currSize.z >= MaxDistanceCheck)
				{
					Logging.Log("Searched too far. Stopping");
					return;
				}

				if (currSize.x >= maxMarkerSize / 2 || currSize.z >= maxMarkerSize / 2)
				{
					Logging.Log("Marker is as big as it gets. Stopping");
					return;
				}

				#endregion

				
				MakeSureMarkerIsInsideMap(ref currPos);

				if (!GetCollisions(ref currPos, ref currSize, out bool forward, out bool left, out bool back, out bool right)) continue;

				var moved = MoveBasedOnCollisions(ref currPos, forward, left, back, right, "");
				var resized = ResizeBasedOnCollisions(ref currSize, forward, left, back, right, "");

				if (!moved || !resized) break;
			}
		}

		/// <summary>
		/// Returns false if the object was found to be inside another object (and was therefore moved from inside this method)
		/// </summary>
		private bool GetCollisions(ref Vector3 currPos, ref Vector3 currSize, out bool collForward, out bool collLeft, out bool collBack, out bool collRight)
		{
			collForward = false;
			collLeft = false;
			collBack = false;
			collRight = false;
			
			Collider[] colliders = Physics.OverlapBox(currPos, currSize, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);
			int collidersSize = colliders.Length;

			if (collidersSize == 0)
			{
				currSize.x += boxCheckDistanceIncreasePerAttempt;
				currSize.z += boxCheckDistanceIncreasePerAttempt;
			}


			string collNames = "";
			foreach (Collider coll in colliders)
			{
				bool isNotInsideOtherObject = CheckCollidingDirections(ref currPos, coll, ref collForward, ref collLeft, ref collBack, ref collRight, false, true);

				if (!isNotInsideOtherObject)
				{
					DoSubCollisionChecking(ref currPos, ref currSize, collidersSize);
					return false;
				}

				if (collNames != "") collNames += ",";
				collNames += " " + coll.name;
			}

			Logging.Log($"Collided with {collNames}");

			return true;
		}
		
		private void DoSubCollisionChecking(ref Vector3 currPos, ref Vector3 currSize, int collidersSize)
		{
			Logging.Log("[SUB COLLISION CHECK]: Started");

			// A separate loop that moves the object out of the colliding object

			var subCollisionCheckIterations = 0;
			
			while (true)
			{
				Logging.Log($"[SUB COLLISION CHECK]: Iteration {subCollisionCheckIterations} | Current position: {currPos.ToString("F4")} | Current Size: {currSize.ToString("F4")}");

				if (subCollisionCheckIterations++ > 100)
				{
					Logging.Log("[SUB COLLISION CHECK]: Too many iterations");
					break;
				}

				Collider[] newColliders = Physics.OverlapBox(currPos, currSize , Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);

				var forward = false;
				var left = false;
				var back = false;
				var right = false;

				foreach (Collider newColl in newColliders)
				{
					CheckCollidingDirections(ref currPos, newColl.transform.position, ref forward, ref left, ref back, ref right, newColl.gameObject, true,  true);

					MoveBasedOnCollisions(ref currPos, forward, left, back, right, "[SUB COLLISION CHECK:]");
				}
			}

			Logging.Log("[SUB COLLISION CHECK]: Ended");
		}

		/// <summary>
		/// Returns true if it moved
		/// </summary>
		private bool MoveBasedOnCollisions(ref Vector3 currPos, bool forward, bool left, bool back, bool right, string stringPrefix)
		{
			int sides = (forward ? 1 : 0) + (left ? 1 : 0) + (back ? 1 : 0) + (right ? 1 : 0);

			string directionString = "";

			switch (sides)
			{
				case 4:
					return false;
				case 3:
					if (!forward)
					{
						directionString = "Back, Left, and Right";
						currPos.z += boxCheckDistanceIncreasePerAttempt;
					}
					else if (!left)
					{
						directionString = "Forward, Back, and Right";
						currPos.x += boxCheckDistanceIncreasePerAttempt;
					}
					else if (!back)
					{
						directionString = "Forward, Left, and Right";
						currPos.z -= boxCheckDistanceIncreasePerAttempt;
					}
					else
					{
						directionString = "Forward, Back, and Left";
						currPos.x -= boxCheckDistanceIncreasePerAttempt;
					}
					break;
				case 2: 
					if (forward && back)
					{
						directionString = "Forward and Back";
					}
					else if (left && right)
					{
						directionString = "Left and Right";
					}
					else
					{
						if (forward)
						{
							currPos.z -= boxCheckDistanceIncreasePerAttempt;

							if (left)
							{
								directionString = "Forward and Left";
								currPos.x += boxCheckDistanceIncreasePerAttempt;
							}
							else
							{
								directionString = "Forward and Right";
								currPos.x -= boxCheckDistanceIncreasePerAttempt;
							}
						}
						else if (back)
						{
							currPos.z += boxCheckDistanceIncreasePerAttempt;

							if (left)
							{
								directionString = "Back and left";
								currPos.x += boxCheckDistanceIncreasePerAttempt;
							}
							else
							{
								directionString = "Back and Right";
								currPos.x -= boxCheckDistanceIncreasePerAttempt;
							}
						}
					}
					break;
				case 1:
					if (forward)
					{
						directionString = "Forward";
						currPos.z -= boxCheckDistanceIncreasePerAttempt;
					}

					else if (left)
					{
						directionString = "Left";

						currPos.x += boxCheckDistanceIncreasePerAttempt;
					}

					else if (back)
					{
						directionString = "Back";

						currPos.z += boxCheckDistanceIncreasePerAttempt;
					}

					else if (right)
					{
						directionString = "Right";

						currPos.x -= boxCheckDistanceIncreasePerAttempt;
					}

					break;
				case 0:
					directionString = "None";
					break;
				default:
					Logging.LogError("This can literally never be called - I do not see how [4 * (0 xor 1)] can ever be outside the range 0-4");
					return false;
			}
			Logging.Log($"{stringPrefix} Collided on the sides: {directionString}");

			return true;
		}
		
		/// <summary>
		/// Returns true if it resized
		/// </summary>
		private bool ResizeBasedOnCollisions(ref Vector3 currSize, bool forward, bool left, bool back, bool right, string stringPrefix)
		{
			int sides = (forward ? 1 : 0) + (left ? 1 : 0) + (back ? 1 : 0) + (right ? 1 : 0);

			string directionString = "";

			switch (sides)
			{
				case 4:
					return false;
				case 3:
					if (!forward)
					{
						directionString = "Back, Left, and Right";
						currSize.z += boxCheckDistanceIncreasePerAttempt;
					}
					else if (!left)
					{
						directionString = "Forward, Back, and Right";
						currSize.x += boxCheckDistanceIncreasePerAttempt;
					}
					else if (!back)
					{
						directionString = "Forward, Left, and Right";
						currSize.z += boxCheckDistanceIncreasePerAttempt;
					}
					else
					{
						directionString = "Forward, Back, and Left";
						currSize.x += boxCheckDistanceIncreasePerAttempt;
					}
					break;
				case 2: 
					if (forward && back)
					{
						directionString = "Forward and Back";
						currSize.x += boxCheckDistanceIncreasePerAttempt;

					}
					else if (left && right)
					{
						directionString = "Left and Right";
						currSize.z += boxCheckDistanceIncreasePerAttempt;

					}
					else
					{
						
						currSize.x += boxCheckDistanceIncreasePerAttempt;
						currSize.z += boxCheckDistanceIncreasePerAttempt;
						
						if (forward)
						{
							if (left)
							{
								directionString = "Forward and Left";
							}
							else
							{
								directionString = "Forward and Right";
							}
						}
						else if (back)
						{
							if (left)
							{
								directionString = "Back and Left";
							}
							else
							{
								directionString = "Back and Right";
							}
						}
					}

					break;
				case 1:
					if (forward)
					{
						directionString = "Forward";
					}

					else if (left)
					{
						directionString = "Left";
					}

					else if (back)
					{
						directionString = "Back";
					}

					else if (right)
					{
						directionString = "Right";
					}

					break;
				case 0:
					directionString = "None";
					break;
				default:
					Logging.LogError("This can literally never be called - I do not see how [4 * (0 xor 1)] can ever be outside the range 0-4");
					return false;
			}
			Logging.Log($"{stringPrefix} Collided on the sides: {directionString}");

			return true;
		}

		private void MakeSureMarkerIsInsideMap(ref Vector3 currPos)
		{
			if (currPos.z + (grid.minimumOpenAreaAroundMarkers.z / 2) <= grid.ZEndPos)
			{
				Logging.Log("Too far back. Moved back into map.");
				currPos.z = grid.ZEndPos + (grid.minimumOpenAreaAroundMarkers.z / 2);
			}
			else if (currPos.z - (grid.minimumOpenAreaAroundMarkers.z / 2) >= grid.ZStartPos)
			{
				Logging.Log("Too far forward. Moved back into map.");
				currPos.z = grid.ZStartPos - (grid.minimumOpenAreaAroundMarkers.z / 2);
			}

			if (currPos.x - (grid.minimumOpenAreaAroundMarkers.x / 2) >= grid.XEndPos)
			{
				Logging.Log("Too far to the right. Moved back into map.");
				currPos.x = grid.XEndPos - grid.minimumOpenAreaAroundMarkers.x / 2;
			}
			else if (currPos.x + (grid.minimumOpenAreaAroundMarkers.x / 2) <= grid.XStartPos)
			{
				Logging.Log("Too far to the left. Moved back into map.");
				currPos.x = grid.XStartPos + (grid.minimumOpenAreaAroundMarkers.x / 2);
			}
		}


		private void InstantiateMarker(string markerName, Vector3 markerPos, Vector3 markerSize)
		{
			// var newMarker = PathfindingMarker.CreateInstance(markerName, ref grid);
			var newMarker = PathfindingMarker.CreateInstance(markerName + " " + grid.markers.Count, ref grid);
			
			newMarker.transform.position = markerPos;
			newMarker.transform.SetParent(gridTransform);

			var newMarkerCollider = newMarker.gameObject.AddComponent<BoxCollider>();
			newMarkerCollider.size = markerSize;
			// newMarkerCollider.isTrigger = true;

			Logging.Log("");
			Logging.Log("");
			Logging.Log("");
			Logging.Log("");
			Logging.Log("INSTANTIATED MARKER #" + (grid.markers.Count - 1) + " at position " + markerPos.ToString("F4") + ", with size " + markerSize.ToString("F4"));
			Logging.Log("");
			Logging.Log("");
			Logging.Log("");
			Logging.Log("");
		}

		/// <summary>
		/// Returns false if it could not decipher a direction, which means it's probably inside the object.
		/// </summary>
		private static bool CheckCollidingDirections(ref Vector3 currMarkerPos, Collider collider, ref bool collForward, ref bool collLeft, ref bool collBack, ref bool collRight, bool allowMultipleDirections, bool logging)
		{
			Vector3 colliderPos = collider.ClosestPointOnBounds(currMarkerPos);
			return CheckCollidingDirections(ref currMarkerPos, colliderPos, ref collForward, ref collLeft, ref collBack, ref collRight, collider.gameObject, allowMultipleDirections, logging);
		}

		private static bool CheckCollidingDirections(ref Vector3 currMarkerPos, Vector3 hitPos, ref bool collForward, ref bool collLeft, ref bool collBack, ref bool collRight, GameObject gObjHit,  bool allowMultipleDirections, bool logging)
		{
			var collidedWithSomething = false;
			
			const float floatLeniency = 0.25f;

			float zDist = hitPos.z - currMarkerPos.z;
			float xDist = hitPos.x - currMarkerPos.x;

			if (allowMultipleDirections)
			{
				CheckIfCollidedOnZAxis(hitPos, ref collForward, ref collBack, gObjHit, logging, zDist, floatLeniency, ref collidedWithSomething);
				CheckIfCollidedOnXAxis(hitPos, ref collLeft, ref collRight, gObjHit, logging, xDist, floatLeniency, ref collidedWithSomething);
			}
			else
			{
				if ( Mathf.Abs(zDist) >= Mathf.Abs(xDist))
				{
					CheckIfCollidedOnZAxis(hitPos, ref collForward, ref collBack, gObjHit, logging, zDist, floatLeniency, ref collidedWithSomething);
				}
				else 
				{
					CheckIfCollidedOnXAxis(hitPos, ref collLeft, ref collRight, gObjHit, logging, xDist, floatLeniency, ref collidedWithSomething);
				}
			}

			return collidedWithSomething;
		}

		private static void CheckIfCollidedOnXAxis(Vector3 hitPos, ref bool collLeft, ref bool collRight, GameObject gObjHit, bool logging, float xDist, float floatLeniency, ref bool collidedWithSomething)
		{
			if (!(Math.Abs(xDist) > floatLeniency)) return;
			if (Math.Sign(xDist) == 1)
			{
				if (logging) Logging.Log(gObjHit.name + " Collided right at position: " + hitPos.ToString("F4"));
				collRight = true;
				collidedWithSomething = true;
			}
			else
			{
				if (logging) Logging.Log(gObjHit.name + " Collided left at position: " + hitPos.ToString("F4"));
				collLeft = true;
				collidedWithSomething = true;
			}
		}

		private static void CheckIfCollidedOnZAxis(Vector3 hitPos, ref bool collForward, ref bool collBack, GameObject gObjHit, bool logging, float zDist, float floatLeniency, ref bool collidedWithSomething)
		{
			if (!(Math.Abs(zDist) > floatLeniency)) return;
			if (Math.Sign(zDist) == 1)
			{
				if (logging) Logging.Log(gObjHit.name + " Collided forward at position: " + hitPos.ToString("F4"));
				collForward = true;
				collidedWithSomething = true;
			}
			else
			{
				if (logging) Logging.Log(gObjHit.name + " Collided back at position: " + hitPos.ToString("F4"));
				collBack = true;
				collidedWithSomething = true;
			}
		}
	}
}