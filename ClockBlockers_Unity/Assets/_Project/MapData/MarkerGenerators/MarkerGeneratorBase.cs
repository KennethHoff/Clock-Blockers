using System.Collections.Generic;

using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	[BurstCompile]
	public abstract class MarkerGeneratorBase : MonoBehaviour
	{
		[Header("Gizmo")]
		[SerializeField]
		protected bool showAffectedArea = true;
		
		[Header("Marker Creation")]
		[Tooltip("Effectively ignores Grid.MinimumOpenAreaAroundMarkers\nTo be removed, and fully replaced by that field")]
		public bool createMarkerNearOrInsideCollisions;
		
		[Tooltip("If you create the marker at the exact position it 'should', then rays can hit things that it realistically shouldn't. This is to offset that.\nAssuming no problems, this will be removed soon.")]
		public float creationHeightAboveFloor = 0;

		protected PathfindingGrid grid;
		protected Transform gridTransform;

		[SerializeField]
		protected LayerMask markerLayerMask;

		public virtual void GenerateAllMarkers()
		{
			Logging.Log("GenerateAllMarkers is not implemented in " + GetType());
		}
		
		public void MergeMarkers()
		{
			Logging.Log("MergeMarkers is not implemented in " + GetType());
		}

		private void Awake()
		{
			grid = GetComponent<PathfindingGrid>();
			gridTransform = grid.transform;
		}

		// public virtual void ClearMarkers()
		// {
		// 	Logging.Log("ClearMarkers is not implemented in " + GetType());
		// }

		public virtual void GenerateMarkerConnections()
		{
			Logging.Log("GenerateMarkerAdjacencies is not implemented in " + GetType());
		}

		public virtual PathfindingMarker FindNearestMarker(Vector3 point)
		{
			Logging.Log($"FindNearestMarker is not implemented in {GetType()}");
			return null;
		}

		public virtual List<PathfindingMarker> RetrieveAllMarkers()
		{
			Logging.Log($"RetrieveAllMarkers is not implemented in {GetType()}");
			return null;
		}
	}
}