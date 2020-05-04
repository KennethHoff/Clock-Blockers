using System.Collections.Generic;

using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators
{
	public abstract class IntervalAutomatedMarkerGenerator : AutomatedMarkerGenerator
	{
		[Space(10)]
		[Range(MinDistance, MaxDistance)]
		public int xDistanceBetweenMarkers;

		[Range(MinDistance, MaxDistance)]
		public int zDistanceBetweenMarkers;
		
		protected const float MinDistance = 1f;
		protected const float MaxDistance = 100f;
		protected float MarkerSizeAdjustedXStartPos => Mathf.Ceil(-grid.XLength / 2 + XDistanceBetweenMarkers/2);
		protected float MarkerSizeAdjustedZStartPos => Mathf.Floor(grid.ZLength / 2 - ZDistanceBetweenMarkers/2);
		private float XDistanceBetweenMarkers => xDistanceBetweenMarkers;
		private float ZDistanceBetweenMarkers => zDistanceBetweenMarkers;
		private int NumberOfColumns => Mathf.FloorToInt(grid.XLength / xDistanceBetweenMarkers)-1;
		private int NumberOfRows => Mathf.FloorToInt(grid.ZLength / zDistanceBetweenMarkers)-1;

		public sealed override void GenerateMarkerAdjacencies()
		{
			foreach (PathfindingMarker marker in grid.markers)
			{
				marker.connectedMarkers = new List<MarkerStats>();
        
				Transform cachedTransform = transform;
        
				Vector3 position = cachedTransform.position;
        
				foreach (PathfindingMarker marker2 in grid.markers)
				{
					if (marker2 == marker) continue;
					Vector3 markerPos = marker2.transform.position;
					
					Vector3 vectorToChild = markerPos - position;
					float distanceToChild = Vector3.Distance(position, markerPos);
        
					if (Physics.Raycast(position, vectorToChild, distanceToChild)) continue;
        
					var markerStat = new MarkerStats(marker2, vectorToChild.magnitude);
					marker.connectedMarkers.Add(markerStat);
				}
        
				marker.PickAColor();
			}
		}

		// public override void GenerateMarkerAdjacencies()
		// {
		// 	Logging.Log("Generating Marker Adjacencies");
		// 	grid.markers.ForEach(marker =>
		// 	{
		// 		if (marker == null) return;
		// 		Vector3 position = marker.transform.position;
		// 		Vector3 gridMinimumOpenAreaAroundMarkers = position + grid.minimumOpenAreaAroundMarkers;
		//
		//
		// 		Collider[] hits = Physics.OverlapBox(position, grid.minimumOpenAreaAroundMarkers);
		//
		// 		Logging.Log("# of hits: " + hits.Length);
		// 		
		// 		List<MarkerStats> connectedMarkers = new List<MarkerStats>();
		// 		
		// 		foreach (Collider hit in hits)
		// 		{
		// 			var collMarker = hit.GetComponent<PathfindingMarker>();
		// 			if (collMarker == null) return;
		//
		// 			var collMarkerStat = new MarkerStats(collMarker, Vector3.Distance(position, gridMinimumOpenAreaAroundMarkers));
		//
		// 			Logging.Log(collMarkerStat.marker + " : " + collMarkerStat.distance);
		// 			connectedMarkers.Add(collMarkerStat);
		// 		}
		// 		
		// 		marker.SetAdjacentMarkers(connectedMarkers);
		// 	});
		// }

		public sealed override void ClearMarkers()
		{
			foreach (PathfindingMarker marker in grid.markers)
			{
				if (marker == null) continue;
				DestroyImmediate(marker.transform.parent.gameObject);
			}
			grid.markers.Clear();
		}

		public sealed override void GenerateAllMarkers()
		{
			if (xDistanceBetweenMarkers < MinDistance || zDistanceBetweenMarkers < MinDistance)
			{
				Logging.LogWarning("X/Z Distance Between Markers is unset.");
				return;
			} 
			
			if (grid.markers == null) grid.markers = new List<PathfindingMarker>(NumberOfColumns * NumberOfRows);
			
			
			for (var i = 0; i <= NumberOfColumns; i ++)
			{
				CreateMarkerColumn(i);
			}
		}
		
		protected void CreateMarkerColumn(int i)
		{
			float xPos = MarkerSizeAdjustedXStartPos + (xDistanceBetweenMarkers * i);

			Transform newColumn = new GameObject("Column " + i).transform;

			newColumn.position = new Vector3(xPos, 0, 0);

			newColumn.SetParent(gridTransform);

			var createdNothing = true;

			for (var j = 0; j <= NumberOfRows; j++)
			{
				if (CreateMarker(j, xPos, newColumn)) createdNothing = false;
				else Logging.Log("Did not create any markers on column " + i + ", row " + j);
			}

			if (!createdNothing) return;
			DestroyImmediate(newColumn.gameObject);
			Logging.Log("Did not create any Markers on column " + i);
		}

		protected abstract bool CreateMarker(int i, float xPos, Transform newColumn);

		protected void InstantiateMarker(string markerName, Vector3 markerPos, ref Transform parent)
		{
			PathfindingMarker.CreateInstance(markerName, ref grid, ref markerPos, ref parent, ref creationHeightAboveFloor);
		}
	}
}