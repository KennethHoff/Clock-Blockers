using System;


namespace ClockBlockers.MapData
{
	public class Node
	{
		public enum NodeState
		{
			Untested, 
			Open,
			Closed
		}
		public Node(PathfindingMarker newMarker, float newG, float newH)
		{
			marker = newMarker;
			g = newG;
			h = newH;
			parentNode = null;
			state = NodeState.Untested;
		}

		public Node(PathfindingMarker newMarker)
		{
			marker = newMarker;
			Reset();
		}

		public void Reset()
		{
			g = float.MaxValue;
			h = float.MaxValue;
			parentNode = null;
			state = NodeState.Untested;
		}

		public void SetDistances(float newG, float newH)
		{
			g = newG;
			h = newH;
		}

		public readonly PathfindingMarker marker;
		
		private float g;
		private float h;

		public NodeState state;

		public Node parentNode;

		public float F => g + h;
		public float G => g;
	}
}