using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;


namespace ClockBlockers.MapData.MarkerGenerators
{
	[BurstCompile]
	public abstract class IntervalAutomatedMarkerGenerator : AutomatedMarkerGenerator
	{

		[Serializable]
		private struct MarkerAndDistanceToZero
		{
			public float distanceToZero;
			public PathfindingMarker marker;
			public MarkerAndDistanceToZero(float distanceToZero, PathfindingMarker marker)
			{
				this.distanceToZero = distanceToZero;
				this.marker = marker;
			}
		}

		enum AdjacencyDirection
		{
			Forward = 0, 
			ForwardRight = 1,
			Right = 2,
			BackRight = 3,
			Back = 4,
			BackLeft = 5,
			Left = 6,
			ForwardLeft = 7,
			Unknown = -1
		}
		
		[Space(10)]
		[Range(MinDistance, MaxDistance)]
		public int xDistanceBetweenCreatedMarkers;

		[Range(MinDistance, MaxDistance)]
		public int zDistanceBetweenCreatedMarkers;

		private const float MinDistance = 1f;
		private const float MaxDistance = 100f;
		private float MarkerSizeAdjustedXStartPos => Mathf.Ceil(-grid.XLength / 2 + XDistanceBetweenCreatedMarkers/2);
		protected float MarkerSizeAdjustedZStartPos => Mathf.Floor(grid.ZLength / 2 - ZDistanceBetweenCreatedMarkers/2);
		private float XDistanceBetweenCreatedMarkers => xDistanceBetweenCreatedMarkers;
		private float ZDistanceBetweenCreatedMarkers => zDistanceBetweenCreatedMarkers;

		private int Columns => Mathf.FloorToInt(grid.XLength / xDistanceBetweenCreatedMarkers);
		private int Rows => Mathf.FloorToInt(grid.ZLength / zDistanceBetweenCreatedMarkers);


		public sealed override void GenerateAllMarkers()
		{
			if (xDistanceBetweenCreatedMarkers < MinDistance || zDistanceBetweenCreatedMarkers < MinDistance)
			{
				Logging.LogWarning("X/Z Distance Between Markers is unset.");
				return;
			} 
			
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>(Columns * Rows);

			for (var i = 0; i < Columns; i ++)
			{
				CreateMarkerColumn(i);
			}

		}

		public sealed override void ClearMarkers()
		{
			foreach (PathfindingMarker marker in grid.markers.Where(marker => marker != null))
			{
				DestroyImmediate(marker.transform.parent.gameObject);
			}

			grid.ClearMarkerList();
		}

		/// <summary>
		/// Returns the amount of markers created on this column
		/// </summary>
		private int CreateMarkerColumn(int i)
		{
			float xPos = MarkerSizeAdjustedXStartPos + (xDistanceBetweenCreatedMarkers * i);

			Transform newColumn = new GameObject("Column " + i).transform;

			newColumn.position = new Vector3(xPos, 0, 0);

			newColumn.SetParent(gridTransform);

			var markersCreatedThisColumn = 0;
			
			for (var j = 0; j < Rows; j++)
			{
				int markersCreatedThisRow = CreateMarker(xPos, j, newColumn, i);
				if (markersCreatedThisRow == 0)
				{
					Logging.Log("Did not create any markers on column " + i + ", row " + j);
					continue;
				}
				
				markersCreatedThisColumn += markersCreatedThisRow;
			}

			if (markersCreatedThisColumn == 0)
			{
				DestroyImmediate(newColumn.gameObject);
				Logging.Log("Did not create any Markers on column " + i);
			}

			return markersCreatedThisColumn;

		}

		/// <summary>
		/// Returns the number of markers created on this x/z-Pos
		/// </summary>
		/// <param name="xPos"></param>
		/// <param name="rowIndex"></param>
		/// <param name="newColumn"></param>
		/// <param name="columnIndex"></param>
		/// <returns></returns>
		protected abstract int CreateMarker(float xPos, int rowIndex, Transform newColumn, int columnIndex);
		
		protected PathfindingMarker InstantiateMarker(string markerName, Vector3 markerPos, Transform parent)
		{
			var newMarker = PathfindingMarker.CreateInstance(markerName, grid, markerPos, parent, creationHeightAboveFloor);
			
			GameObject newGObj = newMarker.gameObject;
			
			var boxCollider = newGObj.AddComponent<BoxCollider>();
			boxCollider.size = grid.minimumOpenAreaAroundMarkers / 2;
			boxCollider.isTrigger = true;

			newGObj.layer = markerLayerMask.GetLayerInt();

			return newMarker;
		}
		
		#region Marker Adjacency

		
		// TODO: MOST IMPORTANT >> Fix up/Down adjacency. << 

		// Does not work well with too large 'Distance between Created Markers'[Basically, anything except 1/2, and even then that's debatable - it's fine for now, I'll probably use 2 anyways]
		public sealed override void GenerateMarkerAdjacencies()
		{
			foreach (PathfindingMarker marker in grid.markers)
			{
				// F	^ = 0 
				// F/R	^> = 1
				// R	> = 2
				// B/R	v> = 3
				// B	v = 4
				// B/L	<v = 5
				// L	< = 6
				// F/L	<^ = 7
				
				// NULL = 'Out of Bounds', or 'Inside Wall'

				var connectedMarkers = new PathfindingMarker[8];
				
				Vector3 markerPos = marker.transform.position;
				
				foreach (PathfindingMarker marker2 in grid.markers)
				{
					if (marker2 == marker) continue;
					
					Vector3 marker2Pos = marker2.transform.position;
					

					float xDist = marker2Pos.x - markerPos.x;
					if (Mathf.Abs(xDist) > xDistanceBetweenCreatedMarkers) continue;
					
					float zDist = marker2Pos.z - markerPos.z;
					if (Mathf.Abs(zDist) > xDistanceBetweenCreatedMarkers) continue;

					if (Math.Abs(xDist) < 0.1f && Math.Abs(zDist) < 0.1f) continue;

					
					float yDist = marker2Pos.y - markerPos.y;

					// Have to be checked prior to the x and y, because those would depend on this. If it's too far above, then it's a fail, but if it's too far below, then it's fine (up to a point - You can fall a lot further than you can jump)
					if (!CheckValidHeightDifference(yDist)) continue;

					AdjacencyDirection direction = FindRelativeDirection(xDist, zDist);
					if (direction == AdjacencyDirection.Unknown)
					{
						Logging.LogWarning($"Unknown direction");
						return;
					}
					
					if (connectedMarkers[(int)direction] != null)
					{
						Logging.LogWarning($"Somehow there's already a marker on {marker}'s connectedMarker {direction} direction");
						continue;
					}

					if (CheckIfColliding(markerPos, direction, xDist, zDist, yDist)) continue;
					
					connectedMarkers[(int)direction] = marker2;
				}
				
				marker.connectedMarkers = connectedMarkers.ToList();
			}
		}

		// Due to how it's constructed, it can never collide for non-Vertical reasons. If there is a X-Z collision, then the marker would simply not be created in the first place
		private bool CheckIfColliding(Vector3 markerPos, AdjacencyDirection direction, float xDist, float zDist, float yDist)
		{
			// 'Marker' is referring to the marker whose 'connectedMarkers' list is being worked on.
			// 'Object' is referring to the marker that we're checking if it is line-of-sight.
			
			// If 'Marker' is above 'Object', that means going down is naturally always going to collide(Due to the fact that all markers are spawned on top of colliders), therefore start with X/Z
			// If 'Marker is underneath 'Object', then casting a ray towards it, along the ground, will naturally always collide as well, for the same reason.
			
			bool yCollides = false;
			bool xCollides = false;
			bool zCollides = false;

			// If negative, that means 'Marker' is above.
			
			// 'Marker' is underneath
			if (Mathf.Abs(yDist-grid.minimumOpenAreaAroundMarkers.y) < 0.1f)
			{
				// y first
				yCollides = CheckIfCollidingOnY(markerPos, yDist);
				
				Vector3 newPos = markerPos;
				newPos.y += yDist;
				
				CheckIfCollidingOnXOrZ(newPos, direction, xDist, zDist, ref zCollides, ref xCollides);

			}
			
			// 'Marker' is above
			else
			{
				// x or z first
				CheckIfCollidingOnXOrZ(markerPos, direction, xDist, zDist, ref zCollides, ref xCollides);
				
				Vector3 newPos = markerPos;
				newPos.x += xDist;
				newPos.z += zDist;

				yCollides = CheckIfCollidingOnY(newPos, yDist);
			}
			
			return yCollides || xCollides || zCollides;
		}

		private void CheckIfCollidingOnXOrZ(Vector3 point, AdjacencyDirection direction, float xDist, float zDist, ref bool zColl, ref bool xColl)
		{
			switch (direction)
			{
				case AdjacencyDirection.Forward:
				case AdjacencyDirection.Back:
					zColl = CheckIfCollidingOnZ(point, zDist);
					xColl = false;
					break;

				case AdjacencyDirection.Left:
				case AdjacencyDirection.Right:
					xColl = CheckIfCollidingOnX(point, xDist);
					zColl = false;
					break;

				case AdjacencyDirection.ForwardRight:
				case AdjacencyDirection.BackRight:
				case AdjacencyDirection.BackLeft:
				case AdjacencyDirection.ForwardLeft:
					// LOW-PRIO: Need to check both X->Z->Y *AND* Z->X->Y
					break;

				case AdjacencyDirection.Unknown:
				default:
					Logging.LogError("Something terrible has happened here");
					break;
			}
		}

		private bool CheckIfCollidingOnASingleAxis(Vector3 pos, float dist, Vector3 direction)
		{
			bool hitSomething = Physics.Raycast(pos, direction, out RaycastHit hit,Mathf.Abs(dist), markerLayerMask.GetLayerInt());
			return hitSomething;
		}

		private bool CheckIfCollidingOnY(Vector3 pos, float dist)
		{
			return CheckIfCollidingOnASingleAxis(pos, dist, dist > 0 ? Vector3.up : Vector3.down);
		}

		private bool CheckIfCollidingOnZ(Vector3 pos, float dist)
		{
			return CheckIfCollidingOnASingleAxis(pos, dist, dist > 0 ? Vector3.forward : Vector3.back);
		}
		
		private bool CheckIfCollidingOnX(Vector3 pos, float dist)
		{
			return CheckIfCollidingOnASingleAxis(pos, dist, dist > 0 ? Vector3.right : Vector3.left);
		}

		private bool CheckValidHeightDifference(float yDist)
		{
			if (yDist > grid.minimumOpenAreaAroundMarkers.y)
				return false;
			
			return true;
		}

		private static AdjacencyDirection FindRelativeDirection(float xDist, float zDist)
		{
			if (Math.Abs(xDist) < 0.1f)
			{
				return zDist > 0 ? AdjacencyDirection.Forward : AdjacencyDirection.Back;
			}

			if (Math.Abs(zDist) < 0.1f)
			{
				return xDist > 0 ? AdjacencyDirection.Right : AdjacencyDirection.Left;
			}

			if (xDist > 0)
			{
				return zDist > 0 ? AdjacencyDirection.ForwardRight : AdjacencyDirection.BackRight;
			}

			if (xDist < 0)
			{
				return zDist > 0 ? AdjacencyDirection.ForwardLeft : AdjacencyDirection.BackLeft;
			}

			return AdjacencyDirection.Unknown;
		}
		#endregion


		// TODO: Is this even necessary? Assuming the agent uses these markers to move, surely they would store where the marker they're currently standing on?
		public override PathfindingMarker FindNearestMarker(Vector3 point)
		{

			// TODO: It does not feel nice using colliders on the markers. Ideally I wouldn't need the markers at all, but that's not for now.
			
			Vector3 increasePerTime = grid.minimumOpenAreaAroundMarkers / 2;
			Vector3 currSize = Vector3.zero;
			
			while (true)
			{
				currSize += increasePerTime;
				if (currSize.x >= 50) return null;
				
				Collider[] overlaps = Physics.OverlapBox(point, currSize, Quaternion.identity, markerLayerMask);
				if (overlaps.Length == 0) continue;

				return overlaps[0].GetComponent<PathfindingMarker>();
			}
		}
	}
}