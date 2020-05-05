﻿using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	public abstract class IntervalAutomatedMarkerGenerator : AutomatedMarkerGenerator
	{

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

		[Space(5)]
		[Range(MinDistance, MaxDistance)]
		public int xDistanceBetweenMergedMarkers;

		[Range(MinDistance, MaxDistance)]
		public int zDistanceBetweenMergedMarkers;

		private const float MinDistance = 1f;
		private const float MaxDistance = 100f;
		private float MarkerSizeAdjustedXStartPos => Mathf.Ceil(-grid.XLength / 2 + XDistanceBetweenCreatedMarkers/2);
		protected float MarkerSizeAdjustedZStartPos => Mathf.Floor(grid.ZLength / 2 - ZDistanceBetweenCreatedMarkers/2);
		private float XDistanceBetweenCreatedMarkers => xDistanceBetweenCreatedMarkers;
		private float ZDistanceBetweenCreatedMarkers => zDistanceBetweenCreatedMarkers;
		private int NumberOfColumns => Mathf.FloorToInt(grid.XLength / xDistanceBetweenCreatedMarkers)-1;
		private int NumberOfRows => Mathf.FloorToInt(grid.ZLength / zDistanceBetweenCreatedMarkers)-1;

		private List<PathfindingMarker> mergedMarkers;

		private List<PathfindingMarker> preMergedMarkers;

		public sealed override void GenerateAllMarkers()
		{
			if (xDistanceBetweenCreatedMarkers < MinDistance || zDistanceBetweenCreatedMarkers < MinDistance)
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

		public sealed override void ClearMarkers()
		{
			foreach (PathfindingMarker marker in grid.markers.Where(marker => marker != null))
			{
				DestroyImmediate(marker.transform.parent.gameObject);
			}

			grid.markers.Clear();
		}

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
					// Logging.Log("Colliding Forwards or Back");
					zColl = CheckIfCollidingOnZ(point, zDist);
					xColl = false;
					break;

				case AdjacencyDirection.Left:
				case AdjacencyDirection.Right:
					// Logging.Log("Colliding Left or Right");

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
			var hitSomething = Physics.Raycast(pos, direction, out RaycastHit hit,Mathf.Abs(dist));
			return hitSomething;
		}

		private bool CheckIfCollidingOnY(Vector3 pos, float dist)
		{
			// Logging.Log("Checking if colliding on Y");

			return CheckIfCollidingOnASingleAxis(pos, dist, dist > 0 ? Vector3.up : Vector3.down);
		}

		private bool CheckIfCollidingOnZ(Vector3 pos, float dist)
		{
			// Logging.Log("Checking if colliding on Z");
			return CheckIfCollidingOnASingleAxis(pos, dist, dist > 0 ? Vector3.forward : Vector3.back);
		}
		
		private bool CheckIfCollidingOnX(Vector3 pos, float dist)
		{
			// Logging.Log("Checking if colliding on X");

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

		private void CreateMarkerColumn(int i)
		{
			float xPos = MarkerSizeAdjustedXStartPos + (xDistanceBetweenCreatedMarkers * i);

			Transform newColumn = new GameObject("Column " + i).transform;

			newColumn.position = new Vector3(xPos, 0, 0);

			newColumn.SetParent(gridTransform);

			var createdNothing = true;

			for (var j = 0; j <= NumberOfRows; j++)
			{
				if (CreateMarker(xPos, j, newColumn, i)) createdNothing = false;
				else Logging.Log("Did not create any markers on column " + i + ", row " + j);
			}

			if (!createdNothing) return;
			DestroyImmediate(newColumn.gameObject);
			Logging.Log("Did not create any Markers on column " + i);
		}

		protected abstract bool CreateMarker(float xPos, int rowIndex, Transform newColumn, int columnIndex);
		
		protected PathfindingMarker InstantiateMarker(string markerName, Vector3 markerPos, Transform parent)
		{
			var newMarker = PathfindingMarker.CreateInstance(markerName, grid, markerPos, parent, creationHeightAboveFloor);
			
			return newMarker;
		}
	}
}