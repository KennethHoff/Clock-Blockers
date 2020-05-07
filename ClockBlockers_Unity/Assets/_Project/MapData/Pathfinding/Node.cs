using System.Collections.Generic;


namespace ClockBlockers.MapData.Pathfinding
{
	public class Node
	{
		public Node(PathfindingMarker newMarker, float newG = float.MaxValue, float newH = float.MaxValue)
		{
			marker = newMarker;
			H = newG;
			G = newH;

			parentNode = null;
			childNodes = new List<Node>();
		}

		public void SetDistances(float newG, float newH)
		{
			G = newG;
			H = newH;
		}

		public readonly PathfindingMarker marker;

		public float H { get; set; }
		public float G { get; set; }

		public float F => G + H;

		public Node parentNode;

		public IEnumerable<Node> childNodes;
	}
}