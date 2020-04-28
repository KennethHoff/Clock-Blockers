using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData
{
	[ExecuteAlways]
	public class PathfindingGrid : MonoBehaviour
	{

		private const float MinDistance = 1f;
		private const float MaxDistance = 100f;

		// public List<Tuple<int, Color>> nodeColorBasedOnAdjacencies = new List<Tuple<int, Color>>();

		[Header("Marker Gizmos")]
		[Tooltip("Less than this, and it will be classified as 'Few'")]
		public int fewAdjacentNodesAmount = 3;
		[Tooltip("Less than this, and it will be classified as 'some'.\nMore than this, but less than 'tooMany', and it will be classified as 'Many'")]
		public int someAdjacentNodesAmount = 10;
		
		[Tooltip("More than this, and it will be classified as 'tooMany', and it will not create ray gizmos.")]
		public int tooManyAdjacentNodesAmount = 50;
		
		public bool alwaysDrawNodes = true;
		public bool alwaysDrawRays = true;

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

		[Header("Grid")]
		[SerializeField]
		private Transform floorPlane;

		[SerializeField]
		private PathfindingMarker markerPrefab;

		public List<PathfindingMarker> markers;

		[SerializeField] [Range(MinDistance, MaxDistance)]
		private int xDistanceBetweenMarkers;

		[SerializeField] [Range(MinDistance, MaxDistance)]
		private int zDistanceBetweenMarkers;

		[SerializeField]
		private bool createMarkersUnderCollisions;

		// I want to only allow integer inputs, but I want to get fractions when dividing
		private float XDistanceBetweenMarkers => xDistanceBetweenMarkers;
		private float ZDistanceBetweenMarkers => zDistanceBetweenMarkers;

		
		
		private void OnDrawGizmos()
		{
			if (Selection.GetFiltered<PathfindingMarker>(SelectionMode.TopLevel).Length == 0) ResetAllMarkerGizmos();
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

		public void GenerateAllMarkers()
		{
			if (xDistanceBetweenMarkers < MinDistance || zDistanceBetweenMarkers < MinDistance) return; 
			Transform thisTransform = transform;
			Vector3 planeLocalScale = floorPlane.transform.localScale;
			
			float xLength = planeLocalScale.x * 10;
			float zLength = planeLocalScale.z * 10;

			float xStartPos = Mathf.Floor(-xLength / 2 + XDistanceBetweenMarkers/2);
			float zStartPos = Mathf.Floor(-zLength / 2 + ZDistanceBetweenMarkers/2);

			int numberOfColumns = Mathf.FloorToInt(xLength / xDistanceBetweenMarkers)-1;
			int numberOfRows = Mathf.FloorToInt(zLength / zDistanceBetweenMarkers)-1;
			
			if (markers == null) markers = new List<PathfindingMarker>(numberOfColumns * numberOfRows);
			
			for (var i = 0; i <= numberOfColumns; i ++)
			{
				CreateMarkerRow(xStartPos, i, thisTransform, numberOfRows, zStartPos);
			}
			
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

		private void CreateMarkerRow(float xStartPos, int i, Transform thisTransform, int numberOfRows, float zStartPos)
		{
			float xPos = xStartPos + (xDistanceBetweenMarkers * i);

			Transform newRow = new GameObject("Row " + i).transform;

			newRow.position = new Vector3(xPos, 0, 0);

			newRow.SetParent(thisTransform);

			var createdNothing = true;

			for (var j = 0; j <= numberOfRows; j++)
			{
				if (CreateMarker(zStartPos, j, xPos, newRow)) createdNothing = false;
			}

			if (createdNothing) DestroyImmediate(newRow.gameObject);
		}

		private bool CreateMarker(float zStartPos, int j, float xPos, Transform newRow)
		{
			float markerScale = nodeScale;
			
			float zPos = zStartPos + (zDistanceBetweenMarkers * j);
			var markerPos = new Vector3(xPos, heightAboveFloor + markerScale, zPos);
			
			if (!createMarkersUnderCollisions && Physics.CheckBox(markerPos, Vector3.one * markerScale * 0.9f)) return false;

			PathfindingMarker newMarker = Instantiate(markerPrefab, markerPos, Quaternion.identity, newRow);
			newMarker.Grid = this;

			newMarker.name = "Column " + j;
			markers.Add(newMarker);
			return true;
		}
	}
}