using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData
{
	public abstract class MarkerGeneratorBase : MonoBehaviour
	{
		protected PathfindingGrid grid;
		protected Transform gridTransform;
		
		[Header("Gizmo")]
		[SerializeField]
		protected bool showAffectedArea = true;
		
		[Header("Marker Creation")]
		
		public bool createMarkerNearOrInsideCollisions;
		
		[Tooltip("If you create the marker at the exact position it 'should', then rays can hit things that it realistically shouldn't. This is to offset that")]
		public float creationHeightAboveFloor = 0;
		
		private void Awake()
		{
			grid = GetComponent<PathfindingGrid>();
			gridTransform = grid.transform;
		}

		public virtual void GenerateAllMarkers()
		{
			Logging.Log("GenerateAllMarkers is not implemented in " + GetType());
		}

		public virtual void ClearMarkers()
		{
			Logging.Log("ClearMarkers is not implemented in " + GetType());

		}

		public virtual void GenerateMarkerAdjacencies()
		{
			Logging.Log("GenerateMarkerAdjacencies is not implemented in " + GetType());
		}
	}

	class PrePlacedPointsMarkerGenerator : MarkerGeneratorBase
	{
		public override void GenerateAllMarkers() { }
	}
}