using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	public abstract class AutomatedMarkerGenerator : MarkerGeneratorBase
	{
		[Header("Gizmo")]
		[SerializeField]
		protected bool showAffectedArea = true;
		
		[Header("Marker Creation")]
		[Tooltip("Effectively ignores Grid.MinimumOpenAreaAroundMarkers\nTo be removed, and fully replaced by that field")]
		public bool createMarkerNearOrInsideCollisions;
		
		[Tooltip("If you create the marker at the exact position it 'should', then rays can hit things that it realistically shouldn't. This is to offset that.\nAssuming no problems, this will be removed soon.")]
		public float creationHeightAboveFloor = 0;

		public virtual void GenerateAllMarkers()
		{
			Logging.Log("GenerateAllMarkers is not implemented in " + GetType());
		}
		
		public virtual void MergeMarkers()
		{
			Logging.Log("MergeMarkers is not implemented in " + GetType());
		}

	}
}