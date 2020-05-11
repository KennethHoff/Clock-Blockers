using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Characters;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;


namespace ClockBlockers.MapData.MarkerGenerators
{
	[BurstCompile]
	public abstract class IntervalMarkerGeneratorBase : MarkerGeneratorBase
	{
		[Space(10)]
		[Range(MinDistance, MaxDistance)]
		public int distanceBetweenCreatedMarkers;

		// [SerializeField][Range(0, 100)]
		// private int maxSlopePercentToBeCountedAsFlat = 10;

		// private float MaxSlopeAmountToBeCountedAsFlat =>(grid.minimumOpenAreaAroundMarkers.y / distanceBetweenCreatedMarkers) * maxSlopePercentToBeCountedAsFlat/100;

		[SerializeField][Header("Performance")]
		private int markerConnectionsToGeneratePerFrame;

		private const float MinDistance = 1f;
		private const float MaxDistance = 100f;
		private float MarkerSizeAdjustedXStartPos => Mathf.Ceil(-grid.XLength / 2 + DistanceBetweenCreatedMarkers/2);
		protected float MarkerSizeAdjustedZStartPos => Mathf.Floor(grid.ZLength / 2 - DistanceBetweenCreatedMarkers/2);
		private float DistanceBetweenCreatedMarkers => distanceBetweenCreatedMarkers;

		private int Columns => Mathf.FloorToInt(grid.XLength / DistanceBetweenCreatedMarkers);
		private int Rows => Mathf.FloorToInt(grid.ZLength / DistanceBetweenCreatedMarkers);
		
		
		private void OnValidate()
		{
			if (markerConnectionsToGeneratePerFrame < 1) markerConnectionsToGeneratePerFrame = 1;
		}

		public sealed override List<PathfindingMarker> RetrieveAllMarkers()
		{
			List<PathfindingMarker> markers = gridTransform.GetComponentsInChildren<PathfindingMarker>().ToList();
			return markers;
		}

		public sealed override void GenerateAllMarkers()
		{
			Logging.Log("Generating markers");
			if (distanceBetweenCreatedMarkers < MinDistance)
			{
				Logging.LogWarning("X/Z Distance Between Markers is unset.");
				return;
			} 
			
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>(Columns * Rows);

			for (var i = 0; i < Columns; i ++)
			{
				int _ = CreateMarkerColumn(i);
			}
		}

		/// <summary>
		/// Returns the amount of markers created on this column
		/// </summary>
		private int CreateMarkerColumn(int i)
		{
			float xPos = MarkerSizeAdjustedXStartPos + (distanceBetweenCreatedMarkers * i);

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

			if (markersCreatedThisColumn != 0) return markersCreatedThisColumn;
			
			DestroyImmediate(newColumn.gameObject);
			Logging.Log("Did not create any Markers on column " + i);

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
		
		// Of note:
		// Because marker connections are uni-directional (Basically, an agent can fall down further than they can jump up), this has to be calculated on each marker for each other marker
		public sealed override void GenerateMarkerConnections()
		{
			
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(GenerateMarkerConnectionsWithCoroutines(), this);
			}
			else
			{
				StartCoroutine(GenerateMarkerConnectionsWithCoroutines());
			}
#else
// I can't think of a scenario where this would ever be called outside the UNITY_EDITOR, but :shrug:
			StartCoroutine(GenerateMarkerConnectionsWithCoroutines());
#endif
			
			Logging.Log("Initialized Marker Connection Generation");
		}

		// This will throw an error if you delete the markers during the coroutine, but that's entirely user-error, and I'm fine with that for now.
		private IEnumerator GenerateMarkerConnectionsWithCoroutines()
		{
			var generationsThisFrame = 0;
			
			foreach (PathfindingMarker marker in grid.markers)
			{
				generationsThisFrame++;

				if (generationsThisFrame % markerConnectionsToGeneratePerFrame == 0)
				{
					yield return null;
				}
				// F	^ = 0 
				// F/R	^> = 1
				// R	> = 2
				// B/R	v> = 3
				// B	v = 4
				// B/L	<v = 5
				// L	< = 6
				// F/L	<^ = 7

				// NULL = 'Out of Bounds', or 'Inside Wall'

				var connectedMarkers = new List<MarkerStats>();

				Vector3 markerPos = marker.transform.position;

				foreach (PathfindingMarker marker2 in grid.markers)
				{
					if (marker2 == marker) continue;

					Vector3 marker2Pos = marker2.transform.position;

					float xDist = marker2Pos.x - markerPos.x;
					if (Mathf.Abs(xDist) > distanceBetweenCreatedMarkers) continue;

					float zDist = marker2Pos.z - markerPos.z;
					if (Mathf.Abs(zDist) > distanceBetweenCreatedMarkers) continue;

					if (Math.Abs(xDist) < 0.1f && Math.Abs(zDist) < 0.1f) continue;


					float yDist = marker2Pos.y - markerPos.y;

					// Have to be checked prior to the x and y, because those would depend on this. If it's too far above, then it's a fail, but if it's too far below, then it's fine (up to a point - You can fall a lot further than you can jump)
					// if (!CheckValidHeightDifference(yDist)) continue;

					AdjacencyDirection direction = FindRelativeDirection(xDist, zDist);
					if (direction == AdjacencyDirection.Unknown)
					{
						Logging.LogWarning("Unknown direction");
						yield break;
					}
					
					if (CheckIfColliding(markerPos, direction, xDist, zDist, yDist)) continue;

					// if (connectedMarkers[(int) direction] != null)
					// {
					// 	Logging.LogWarning($"Somehow there's already a marker on {marker}'s connectedMarker {direction} direction");
					// 	continue;
					// }

					connectedMarkers.Add(new MarkerStats(marker2, yDist, direction));

					// connectedMarkers[(int) direction] = new MarkerStats(marker2, yDist);
				}

				marker.connectedMarkers = connectedMarkers;
			}
		}

		// Due to how it's constructed, it can never collide purely on the horizontal axis. If there is a X-Z collision, then the marker would simply not be created in the first place
		private static bool CheckIfColliding(Vector3 markerPos, AdjacencyDirection direction, float xDist, float zDist, float yDist)
		{
			// 'Marker' is referring to the marker whose 'connectedMarkers' list is being worked on.
			// 'Object' is referring to the marker that we're checking line-of-sight towards.
			
			// Keep this in mind while reading this:
			// 'Marker' and 'Object' *are* next to each other, that's how they were created.
			
			// Small illustration: [O = 'Object', M = 'Marker', C = Collider that the marker was created on (C will always be there)]
			
			// In the first scenario, if M cast a ray downwards, it'll always collide with C. Therefore, we need to start by casting the ray 'along the floor' before going down.
			// Conversely, in the second scenario, casting a ray along the floor will always collide with C. Therefore, we need to start by casting the ray upwards

			// 	M ->					O <--
			//		v						^
			// 	C	O					C	M
			
			var yCollides = false;
			var xCollides = false;
			var zCollides = false;


			// 'Object' is above
			if (yDist > 0)
			{
				// y first
				yCollides = CheckIfCollidingVertical(markerPos, yDist);
				
				if (yCollides) return true;
			
				Vector3 newPos = markerPos;
				newPos.y += yDist;
			
				CheckIfCollidingHorizontal(newPos, direction, xDist, zDist, ref zCollides, ref xCollides);

			}
		
			// 'Object' is underneath
			else
			{
				// x or z first
				CheckIfCollidingHorizontal(markerPos, direction, xDist, zDist, ref zCollides, ref xCollides);

				if (zCollides || xCollides) return true;
			
				Vector3 newPos = markerPos;
				newPos.x += xDist;
				newPos.z += zDist;

				yCollides = CheckIfCollidingVertical(newPos, yDist);
			}

			
			return yCollides || xCollides || zCollides;
		}

		private static void CheckIfCollidingHorizontal(Vector3 point, AdjacencyDirection direction, float xDist, float zDist, ref bool zColl, ref bool xColl)
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

		private static bool CheckIfCollidingVertical(Vector3 pos, float dist)
		{
			var ray = new Ray(pos, dist > 0 ? Vector3.up : Vector3.down);
			return RayCaster.CastRay(ray, Mathf.Abs(dist));
		}

		private static bool CheckIfCollidingOnZ(Vector3 pos, float dist)
		{
			var ray = new Ray(pos, dist > 0 ? Vector3.forward : Vector3.back);
			return RayCaster.CastRay(ray, Mathf.Abs(dist));
		}
		
		private static bool CheckIfCollidingOnX(Vector3 pos, float dist)
		{
			var ray = new Ray(pos, dist > 0 ? Vector3.right : Vector3.left);

			return RayCaster.CastRay(ray, Mathf.Abs(dist));
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