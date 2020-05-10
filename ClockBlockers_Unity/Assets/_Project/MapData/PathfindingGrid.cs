using System;
using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.MapData.MarkerGenerators;
using ClockBlockers.MapData.Pathfinding.PathfindingManagement;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;
namespace ClockBlockers.MapData
{
	
	// TODO: Separate into smaller components
	[ExecuteAlways][BurstCompile]
	public class PathfindingGrid : MonoBehaviour
	{
		[Header("Marker Gizmos")]
		[Space(5)]
		public bool alwaysDrawNodes = true;
		public bool alwaysDrawRays = true;

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

		[NonSerialized]
		public IPathfindingManager pathfindingManager;
		
		private MarkerGeneratorBase markerGenerator;

		private void Start()
		{
			if (markers != null && markers.Count > 0) return;
			
			RetrieveMarkers();

			if (markers.Count == 0)
			{
				GenerateMarkers();
			}
		}



		// private void Start()
		// {
		// 	if (!Application.isPlaying) return;
		// 	
		// 	
		// 	// As a band-aid solution to a problem I've found about serialization: You can't Serialize a List of GameObjects
		// 	// I tried storing the instance ID of all the GameObjects, but you can't retrieve objects based on their InstanceID, so it's pointless.
		// 	
		// 	// This is far from ideal, but it works fine *for now*.
		// 	
		//
		// 	// Logging.Log("Start! Yay");
		// 	// ClearMarkers();
		// 	// GenerateMarkers();
		// 	// GenerateMarkerConnections();
		// }
		
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (UnityEditor.Selection.GetFiltered<PathfindingMarker>(UnityEditor.SelectionMode.TopLevel).Length == 0) ResetMarkerGizmos();
			
			if (CheckPathfindingManager()) pathfindingManager.FindPaths();
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
				ClearMarkers();
				break;
			}
		}


#endif
		private bool CheckPathfindingManager()
		{
			if (pathfindingManager != null) return true;
			
			pathfindingManager = GetComponent<IPathfindingManager>();

			return pathfindingManager != null;
		}

		private bool CheckMarkerGenerator()
		{
			if (markerGenerator != null) return true;
			markerGenerator = GetComponent<MarkerGeneratorBase>();
			
			return markerGenerator != null;
		}

		public void ClearMarkers()
		{
			ClearMarkerList();
			DestroyAllChildGameObjects();
		}

		private void DestroyAllChildGameObjects()
		{
			for(int x = transform.childCount -1; x >= 0;x--)
			{
				GameObject child = transform.GetChild(x).gameObject;
#if UNITY_EDITOR
				if (!Application.isPlaying)
				{
					DestroyImmediate(child);
				}
				else
				{
					Destroy(child);
				}
#else
				Destroy(child);
#endif
			}
		}

		public void GetPath(PathfindingMarker marker1, PathfindingMarker marker2, IPathRequester pathRequester, float maxJumpHeight)
		{
			CheckPathfindingManager();
			pathfindingManager.RequestPath(pathRequester, marker1, marker2, maxJumpHeight);
		}

		private void ClearMarkerList()
		{
			markers.Clear();
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


		// [ContextMenu("Request Path from Every Node to Every Other Node")]
		// public void RequestPathFromEveryNodeToEveryOtherNode()
		// {
		// 	Logging.Log("Starting the Request:");
		// 	foreach (PathfindingMarker marker1 in markers)
		// 	{
		// 		foreach (PathfindingMarker marker2 in markers)
		// 		{
		// 			GetPath(marker1, marker2, marker1, defaultJumpHeight);
		// 		}
		//
		// 	}
		// }

		public PathfindingMarker FindNearestMarker(Vector3 point)
		{
			if (!CheckMarkerGenerator()) return null;

			return markerGenerator.FindNearestMarker(point);
		}

		public void GenerateMarkers()
		{
			if (!CheckMarkerGenerator()) return;
			
			markerGenerator.GenerateAllMarkers();
		}

		public void GenerateMarkerConnections()
		{
			if (!CheckMarkerGenerator()) return;
			
			markerGenerator.GenerateMarkerConnections();
		}
		public void RetrieveMarkers()
		{
			if (!CheckMarkerGenerator()) return;
			
			markers = markerGenerator.RetrieveAllMarkers();
		}
	}
}