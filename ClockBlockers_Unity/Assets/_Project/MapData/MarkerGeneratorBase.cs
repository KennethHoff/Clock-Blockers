using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData
{
	public abstract class MarkerGeneratorBase : MonoBehaviour
	{
		protected PathfindingGrid grid;
		protected Transform gridTransform;
		
		[SerializeField]
		protected bool showAffectedArea = true;
		
		

		[Tooltip("The layer the markers themselves encompass")]
		public LayerMask pathfindingMarkerLayer;
		
		public int LayerInt => DataManipulation.GetLayerIntFromLayerMask(pathfindingMarkerLayer);

		private void Awake()
		{
			grid = GetComponent<PathfindingGrid>();
			gridTransform = grid.transform;
		}
		
		public abstract void GenerateAllMarkers();
		public abstract void ClearMarkers();

		public virtual void ResetAllMarkerGizmos()
		{
			Logging.Log("Reset All Marker Gizmos is not implemented in" + GetType());
		}
	}
}