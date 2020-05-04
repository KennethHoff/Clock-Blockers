using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	public abstract class MarkerGeneratorBase : MonoBehaviour
	{
		protected PathfindingGrid grid;
		protected Transform gridTransform;

		private void Awake()
		{
			grid = GetComponent<PathfindingGrid>();
			gridTransform = grid.transform;
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
}