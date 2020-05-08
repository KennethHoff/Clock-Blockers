using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	public abstract class MarkerGeneratorBase : MonoBehaviour
	{
		
		protected PathfindingGrid grid;
		protected Transform gridTransform;

		[SerializeField]
		protected LayerMask markerLayerMask;

		private void Awake()
		{
			grid = GetComponent<PathfindingGrid>();
			gridTransform = grid.transform;
		}

		public virtual void ClearMarkers()
		{
			Logging.Log("ClearMarkers is not implemented in " + GetType());
		}
		
		public virtual void GenerateMarkerConnections()
		{
			Logging.Log("GenerateMarkerAdjacencies is not implemented in " + GetType());
		}

		public virtual PathfindingMarker FindNearestMarker(Vector3 point)
		{
			Logging.Log($"FindNearestMarker is not implemented in {GetType()}");
			return null;
		}
	}
}