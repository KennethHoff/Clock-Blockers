using System.Collections.Generic;

using ClockBlockers.MapData.Marker;
using ClockBlockers.MapData.Marker_Generators;
using ClockBlockers.Utility;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData.Grid
{
	// TODO: Clean up this, separate into components
	
	// The performance of this class' functions is not super-important, as it's used to 'bake' the Pathfinding Grid
	
	// This currently only allows one size of agent, or rather; It creates nodes based on a size.
	// Any smaller agent will be able to follow it, albeit stupidly - It might go around somewhere where it could realistically go through.
	// Any larger agent might be able to follow it, but no guarantees. It might simply be too large to follow accurately
	[ExecuteAlways]
	[RequireComponent(typeof(IMarkerGenerator))]
	public class PathfindingGrid : MonoBehaviour
	{
		#region Fields and Properties

		

		private IMarkerGenerator markerGenerator;

		public const float MinDistance = 0.1f;
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

		// public PathfindingMarker markerPrefab;
		
		[Tooltip("Only creates markers on top of items with these Physics Layers")]
		public LayerMask pathfindingLayer;

		[Tooltip("Unaffected by collision checks")]
		public LayerMask nonCollidingLayer;

		public List<PathfindingMarker> markers;

		// public List<PathfindingMarkerRow> markerRows;
		
		// [Range(MinDistance, MaxDistance)]
		public float xDistanceBetweenMarkers;

		// [Range(MinDistance, MaxDistance)]
		public float zDistanceBetweenMarkers;
		
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

		/// <summary>
		/// Maximum number of created columns
		/// </summary>
		public int MaximumNumberOfColumns => Mathf.FloorToInt(XLength / xDistanceBetweenMarkers)-1;
		/// <summary>
		/// Maximum number of created rows
		/// </summary>
		public int MaximumNumberOfRows => Mathf.FloorToInt(ZLength / zDistanceBetweenMarkers)-1;

		#endregion

		
		private void OnValidate()
		{
			xDistanceBetweenMarkers = xDistanceBetweenMarkers.Clamp(MinDistance, MaxDistance);
			zDistanceBetweenMarkers = zDistanceBetweenMarkers.Clamp(MinDistance, MaxDistance);
		}

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
			foreach (PathfindingMarker checkedMarker in markers)
			{
				if (checkedMarker == null) continue;
				DestroyImmediate(checkedMarker.transform.parent.gameObject);
			}

			markers.Clear();
		}

		public void GenerateMarkerAdjacencies()
		{
			foreach (PathfindingMarker checkedMarker in markers)
			{
				checkedMarker.GetAdjacentMarkersFromGrid();
			}
		}
		
		public void ResetAllMarkerGizmos()
		{
			foreach (PathfindingMarker checkedMarker in markers)
			{
				checkedMarker.ResetGizmo();
			}
		}

	}
}