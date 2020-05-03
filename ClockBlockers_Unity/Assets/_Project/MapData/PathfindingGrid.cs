using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace ClockBlockers.MapData
{
	// TODO: Clean up this, separate into components
	
	// This currently only allows one size of agent, or rather; It creates nodes based on a size.
	// Any smaller agent will be able to follow it, albeit stupidly - It might go around somewhere where it could realistically go through.
	// Any larger agent might be able to follow it, but no guarantees. It might simply be too large to follow accurately
	
	// TODO: Remove the idea of 'row' entirely, and make it more manual. Currently there are significant issues with altitude changes. << Do this after making the pathfinding work between 'connected nodes'.
	[ExecuteAlways]
	public class PathfindingGrid : MonoBehaviour
	{
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

		[Header("Grid Generation")]
		public Transform floorPlane;
		
		[SerializeField]
		public Vector3 minimumOpenAreaAroundMarkers;

		public float XStartPos => transform.position.x - XLength / 2;
		public float XEndPos => transform.position.x + XLength / 2;

		
		public float ZStartPos => transform.position.z + ZLength / 2;
		public float ZEndPos => transform.position.z - ZLength / 2;

		// [Tooltip("Only creates markers on top of items with these Layers")]
		// public LayerMask pathfindingLayer;

		public List<PathfindingMarker> markers;

		private Vector3 PlaneLocalScale => floorPlane.transform.localScale;
		public float XLength => PlaneLocalScale.x;
		public float ZLength => PlaneLocalScale.z;

		private void OnDrawGizmos()
		{
			if (Selection.GetFiltered<PathfindingMarker>(SelectionMode.TopLevel).Length == 0) ResetMarkerGizmos();
		}

		// TODO: Create a new 'Marker Generator' that does not actually create markers, but rather adjusts pre-placed markers.
		
		// Basically, instead of trying to find a way to create markers programatically, instead you have to manually place them around the room,
		// and then use a program to change their size, and do minor positional adjustments

		public void ResetMarkerGizmos()
		{
			foreach (PathfindingMarker marker in markers)
			{
				marker.ResetGizmo();
			}
		}
	}
}