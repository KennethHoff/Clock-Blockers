using System;
using System.Collections.Generic;

using ClockBlockers.Utility;

using UnityEditor;

using UnityEngine;

namespace ClockBlockers.MapData
{
	// TODO: Clean up this, separate into components
	
	// This currently only allows one size of agent, or rather; It creates nodes based on a size.
	// Any smaller agent will be able to follow it, albeit stupidly - It might go around somewhere where it could realistically go through.
	// Any larger agent might be able to follow it, but no guarantees. It might simply be too large to follow accurately
	[ExecuteAlways]
	[RequireComponent(typeof(IMarkerGenerator))]
	public class PathfindingGrid : MonoBehaviour
	{
		private IMarkerGenerator markerGenerator;

		public const float MinDistance = 1f;
		private const float MaxDistance = 100f;

		[Header("Marker Gizmos")]
		[Tooltip("Less than this, and it will be classified as 'Few'")]
		public int fewAdjacentNodesAmount = 3;
		[Tooltip("Less than this, and it will be classified as 'some'.\nMore than this, but less than 'tooMany', and it will be classified as 'Many'")]
		public int someAdjacentNodesAmount = 10;
		
		[Tooltip("More than this, and it will be classified as 'tooMany', and it will not create ray gizmos.")]
		public int tooManyAdjacentNodesAmount = 50;
		
		[Space(5)]
		public bool alwaysDrawNodes = true;
		public bool alwaysDrawRays = true;
		public bool alwaysDrawCollisionArea = false;

		[Space(5)]
		public float heightAboveFloor = 0.25f;
		
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

		[Header("Grid Generation")]
		public Transform floorPlane;

		public PathfindingMarker markerPrefab;
		
		[Tooltip("Only creates markers on top of items with these Physics Layers")]
		public LayerMask pathfindingLayer;

		[Tooltip("Unaffected by collision checks")]
		public LayerMask nonCollidingLayer;

		public List<PathfindingMarker> markers;
		
		[Range(MinDistance, MaxDistance)]
		public int xDistanceBetweenMarkers;

		[Range(MinDistance, MaxDistance)]
		public int zDistanceBetweenMarkers;
		
		[SerializeField]
		public Vector3 minimumOpenAreaAroundMarkers;
		
		public bool createMarkerNearOrInsideCollisions;

		// I want to only allow integer inputs, but I want to get fractions when dividing
		private float XDistanceBetweenMarkers => xDistanceBetweenMarkers;
		private float ZDistanceBetweenMarkers => zDistanceBetweenMarkers;

		private Vector3 PlaneLocalScale => floorPlane.transform.localScale;
		public float XLength => PlaneLocalScale.x * 10;
		public float ZLength => PlaneLocalScale.z * 10;

		public float XStartPos => Mathf.Floor(-XLength / 2 + XDistanceBetweenMarkers/2);
		public float ZStartPos => Mathf.Floor(-ZLength / 2 + ZDistanceBetweenMarkers/2);

		public int NumberOfColumns => Mathf.FloorToInt(XLength / xDistanceBetweenMarkers)-1;
		public int NumberOfRows => Mathf.FloorToInt(ZLength / zDistanceBetweenMarkers)-1;

		private void OnDrawGizmos()
		{
			if (markerGenerator == null) markerGenerator = GetComponent<IMarkerGenerator>();
			
			if (Selection.GetFiltered<PathfindingMarker>(SelectionMode.TopLevel).Length == 0) ResetAllMarkerGizmos();
		}

		public void GenerateAllMarkers()
		{
			markerGenerator.GenerateAllMarkers();
		}

		public void ClearMarkers()
		{
			foreach (PathfindingMarker marker in markers)
			{
				if (marker == null) continue;
				DestroyImmediate(marker.transform.parent.gameObject);
			}

			markers.Clear();
		}

		public void GenerateMarkerAdjacencies()
		{
			foreach (PathfindingMarker marker in markers)
			{
				marker.GetAdjacentMarkersFromGrid();
			}
		}
		
		public void ResetAllMarkerGizmos()
		{
			foreach (PathfindingMarker marker in markers)
			{
				marker.scale = nodeScale;
				marker.PickAColor();
			}
		}
	}
}