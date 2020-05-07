using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using Sisus.OdinSerializer.Utilities;

using UnityEngine;


namespace ClockBlockers.MapData.Pathfinding
{
	[DisallowMultipleComponent]
	
	// https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2
	class AStarPathFinderTwo : MonoBehaviour, IPathfinder
	{
		private class Node
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

			public List<Node> childNodes;

		}
		private Dictionary<int, Node> markerNodeDictionary;
		
		private void AddToMarkerNodeDictionary(PathfindingMarker marker, Node node)
		{
			CheckIfDictionaryExists();
			markerNodeDictionary.Add(marker.GetInstanceID(), node);
		}

		private bool CheckIfDictionaryExists()
		{
			if (markerNodeDictionary != null) return true;
			
			markerNodeDictionary = new Dictionary<int, Node>();
			return false;
		}

		private void ResetDictionary()
		{
			if (markerNodeDictionary == null)
			{
				markerNodeDictionary = new Dictionary<int, Node>();
				return;
			}
			markerNodeDictionary.Clear();
		}

		private Node GetNodeFromMarker(PathfindingMarker marker)
		{
			if (!CheckIfDictionaryExists()) return null;
			
			markerNodeDictionary.TryGetValue(marker.GetInstanceID(), out Node node);
			return node;
		}

		private Node GetOrAddMarkerToDictionary(PathfindingMarker marker)
		{
			Node node = GetNodeFromMarker(marker);
			if (node != null)
			{
				return node;
			}
			
			node = new Node(marker);
			AddToMarkerNodeDictionary(marker, node);
			return node;
		}
		
		private List<Node> TurnConnectedMarkersIntoNodes(PathfindingMarker startMarker)
		{
			var result = new List<Node>(startMarker.ConnMarkerCount);

			startMarker.connectedMarkers.Where(currMarker => currMarker != null).ForEach(connMarker =>
			{
				Node currNode = GetOrAddMarkerToDictionary(connMarker);

				result.Add(currNode);
			});
			return result;
		}
		
		public List<PathfindingMarker> GetPath(PathfindingMarker startMarker, PathfindingMarker endMarker)
		{
			
			ResetDictionary();
			
			var openList = new List<Node>();
			var closedList = new List<Node>();
			
			var path = new List<Node>();

			Node startNode = GetOrAddMarkerToDictionary(startMarker);
			Node endNode = GetOrAddMarkerToDictionary(endMarker);
			
			float distFromStartToEnd = Vector3.Distance(startMarker.transform.position, endMarker.transform.position);

			startNode.G = 0;
			startNode.H = distFromStartToEnd;

			endNode.G = distFromStartToEnd;
			endNode.H = 0;


			openList.Add(startNode);

			while (openList.Count > 0)
			{
				if (openList.Count > 1)
				{
					openList.Sort((node1, node2) => node1.F.CompareTo(node2.F));
				}

				Node currNode = openList.First();
				
				openList.Remove(currNode);
				closedList.Add(currNode);

				if (currNode == endNode)
				{
					Node current = currNode;
					
					while (current != null)
					{
						path.Add(current);
						current = current.parentNode;
					}
					
					path.Reverse();
					break;
				}

				Vector3 currMarkerPos = currNode.marker.transform.position;

				currNode.childNodes = TurnConnectedMarkersIntoNodes(currNode.marker);
				
				foreach (Node childNode in currNode.childNodes)
				{
					if (closedList.Contains(childNode)) continue;
					
					Vector3 childMarkerPos = childNode.marker.transform.position;

					
					
					// TODO: Redo this into something .. better (Currently it randomly goes diagonally back and forth, since that's equally "good")
					float tempG = currNode.G + Vector3.Distance(childMarkerPos, currMarkerPos);



					float tempH = Vector3.Distance(childMarkerPos, endNode.marker.transform.position).Round(4);

					
					if (openList.Contains(childNode))
					{
						// If not set, then it should be float.MaxValue (Although, it shouldn't not be set if it's in the openList)
						if (childNode.G > tempG)
						{
							childNode.parentNode = currNode;
							childNode.G = tempG;
						}
						continue;
					}
					
					childNode.parentNode = currNode;
					childNode.G = tempG;
					childNode.H = tempH;
					
					openList.Add(childNode);
				}
			}
			
			Logging.Log($"The path from {startMarker} to {endMarker} is:");

			foreach (Node currNode in path)
			{
				Logging.Log($"{currNode.marker.name} has F: {currNode.F}, G: {currNode.G}, H: {currNode.H}");
			}

			return path.ConvertAll(node => node.marker);
		}

		public PathfindingGrid Grid { get; set; }
	}
}