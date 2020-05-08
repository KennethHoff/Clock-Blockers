using System;
using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.MapData.MarkerGenerators;
using ClockBlockers.MapData.Pathfinding.PathfindingManagement;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEditor;

using UnityEngine;

namespace ClockBlockers.MapData
{
	// TODO: Clean up this, separate into components
	
	// This currently only allows one size of agent, or rather; It creates nodes based on a size.
	// Any smaller agent will be able to follow it, albeit stupidly - It might go around somewhere where it could realistically go through.
	// Any larger agent might be able to follow it, but no guarantees. It might simply be too large to follow accurately
	
	// TODO: Remove the idea of 'row' entirely, and make it more manual. Currently there are significant issues with altitude changes. << Do this after making the pathfinding work between 'connected nodes'.
	[ExecuteAlways][BurstCompile]
	public class PathfindingGrid : MonoBehaviour
	{
		[Header("Marker Gizmos")]
		[Space(5)]
		public bool alwaysDrawNodes = true;
		public bool alwaysDrawRays = true;
		public bool alwaysDrawCollisionArea = false;

		[Space(5)]
		public float drawHeightAboveFloor = 0.25f;
		
		[Range(0, 2)]
		public float nodeScale = 0.5f;

		[Header("Marker Selection")]
		[Tooltip("Whether or not selecting a node will affect the scale of said node and its adjacent nodes")]
		public bool selectionChangeScale = true;
		
		[Tooltip("Whether or not selecting a node will affect the color of said node and its adjacent nodes")]
		public bool selectionChangeNodeColors = true;
		
		[Tooltip("Whether or not selecting a node will create rays between the node and adjacent nodes")]
		public bool selectionDrawRays = true;

		public bool AffectNodes => selectionChangeScale || selectionChangeNodeColors;

		[Range(0, 2)]
		public float selectionNodeScale = 1f;
		
		[Tooltip("The scale of the node when it's being searched for by a Pathfinder")]
		[Header("Pathfinding Gizmo")]
		public float searchedNodeScale = 0.75f;

		[Header("Grid Generation")]
		public Transform floorPlane;

		private Vector3 FloorPlanePosition => floorPlane.transform.position;

		[SerializeField]
		public Vector3 minimumOpenAreaAroundMarkers;

		public float XStartPos => FloorPlanePosition.x - XLength / 2;
		public float XEndPos => FloorPlanePosition.x + XLength / 2;

		public float ZStartPos => FloorPlanePosition.z + ZLength / 2;
		public float ZEndPos => FloorPlanePosition.z - ZLength / 2;

		[SerializeField]
		private float mapHeight = 20;

		public float MapHeight => mapHeight;

		public FloatReference defaultJumpHeight;

		public float YStartPos => FloorPlanePosition.y;
		public float YEndPos => YStartPos + MapHeight;


		public List<PathfindingMarker> markers;

		private Vector3 PlaneLocalScale => floorPlane.transform.localScale * 10;
		public float XLength => PlaneLocalScale.x;
		public float ZLength => PlaneLocalScale.z;
		
		[HideInInspector]
		public IPathfindingManager pathfindingManager;
		
		[HideInInspector]
		public MarkerGeneratorBase markerGenerator;

		private void OnDrawGizmos()
		{
			if (Selection.GetFiltered<PathfindingMarker>(SelectionMode.TopLevel).Length == 0) ResetMarkerGizmos();
			
			if (CheckPathfindingManager()) pathfindingManager.FindPaths();
		}

		private bool CheckPathfindingManager()
		{
			if (pathfindingManager != null) return true;
			
			pathfindingManager = GetComponent<IPathfindingManager>();

			return pathfindingManager != null;
		}

		private bool CheckMarkerGenerator()
		{
			if (markerGenerator != null) return false;
			markerGenerator = GetComponent<MarkerGeneratorBase>();
			
			return markerGenerator != null;
		}

		public void ResetMarkerGizmos()
		{
			foreach (PathfindingMarker marker in markers)
			{
				if (marker != null)
				{
					marker.ResetGizmo();
					continue;
				}
				
				Logging.LogWarning("A marker was null, which means they're all probably null. Clearing list");
				markers.Clear();
				break;
			}
		}

		public void GetPath(IPathRequester pathRequester, PathfindingMarker marker1, PathfindingMarker marker2, float maxJumpHeight)
		{
			CheckPathfindingManager();
			pathfindingManager.RequestPath(pathRequester, marker1, marker2, maxJumpHeight);
		}

		public void ClearMarkerList()
		{
			markers.Clear();
		}

		public PathfindingMarker FindNearestMarker(Vector3 point)
		{
			if (!CheckMarkerGenerator()) return null;

			return markerGenerator.FindNearestMarker(point);
		}

		public bool CheckIfPointIsInsideMap(Vector3 point)
		{
			if (point.x > XEndPos) return false;
			if (point.x < XStartPos) return false;
			
			if (point.z < ZEndPos) return false;
			if (point.z > ZStartPos) return false;

			if (point.y < YStartPos) return false;
			if (point.y > YEndPos) return false;

			return true;
		}
	}
}