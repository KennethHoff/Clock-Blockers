using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using Sisus.OdinSerializer.Utilities;

using UnityEngine;


namespace ClockBlockers.MapData.Pathfinding
{
	// https://web.archive.org/web/20170505034417/http://blog.two-cats.com/2014/06/a-star-example/
	[DisallowMultipleComponent]
	internal class AStarPathFinderOld : MonoBehaviour, IPathfinder
	{
		
		private PathfindingMarker startMarker;
		private PathfindingMarker endMarker;
		
		public static AStarPathFinderOld CreateInstance(PathfindingMarker startMarker, PathfindingMarker endMarker)
		{
			return new AStarPathFinderOld(startMarker, endMarker);
		}

		private AStarPathFinderOld(PathfindingMarker startMarker, PathfindingMarker endMarker)
		{
			this.startMarker = startMarker;
			this.endMarker = endMarker;
		}
		
		private class Node
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

			public float G
			{
				get => g;
				set => g = value;
			}

			public float H
			{
				get => h;
				set => h = value;
			}
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

		private Node GetNodeFromMarker(PathfindingMarker marker)
		{
			if (!CheckIfDictionaryExists()) return null;
			
			markerNodeDictionary.TryGetValue(marker.GetInstanceID(), out Node node);
			return node;
		}

		private Node GetOrAddMarkerToDictionary(PathfindingMarker marker, float checkedDistToStart, float checkedDistToEnd)
		{
			Node node = GetNodeFromMarker(marker);
			if (node != null)
			{
				node.SetDistances(checkedDistToStart, checkedDistToEnd);
				return node;
			}
			
			node = new Node(marker, checkedDistToStart, checkedDistToEnd);
			AddToMarkerNodeDictionary(marker, node);
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

		private void ResetDictionary()
		{
			if (markerNodeDictionary == null)
			{
				markerNodeDictionary = new Dictionary<int, Node>();
				return;
			}
			foreach (KeyValuePair<int,Node> keyValuePair in markerNodeDictionary)
			{
				keyValuePair.Value.Reset();
			}
		}
		
		private List<Node> TurnConnectedMarkersIntoNodes(PathfindingMarker startMarker, Vector3 startPos, Vector3 endPos)
		{
			var result = new List<Node>(startMarker.ConnMarkerCount);

			startMarker.connectedMarkers.Where(currMarker => currMarker != null).ForEach(connMarker =>
			{
				Vector3 currMarkerPos = connMarker.transform.position;
				
				// float currDistToStart = Vector3.Distance(currMarkerPos, startPos);
				// float currDistToEnd = Vector3.Distance(currMarkerPos, endPos);

				Node currNode = GetOrAddMarkerToDictionary(connMarker);
				// Node node = Grid.GetOrAddMarkerToDictionary(connMarker);

				result.Add(currNode);
			});
			return result;
		}
		
		private List<Node> GetAvailableConnectedNodes(Node fromNode, List<Node> tempNodes, Vector3 endPos)
		{
			var result = new List<Node>();
			
			foreach (Node node in tempNodes)
			{
				// If it's closed, that means it has been in a path at some point (Current path, or rejected). If it's in the current path, then obviously don't check it, and if it's not in the current path, that means the rejected path is worse.
				if (node.state == Node.NodeState.Closed) continue;

				Vector3 fromNodePos = fromNode.marker.transform.position;
				
				// If it's open, that means it's open for consideration; Only added if the new path is shorter than the previous
				if (node.state == Node.NodeState.Open)
				{
					float gTemp = fromNode.G + Vector3.Distance(fromNodePos, node.marker.transform.position);
					if (gTemp >= node.G) continue;
					
					node.parentNode = fromNode;
					result.Add(node);
				}
				// If it hasn't been checked before, that means the new path has to be shorter than the previous.
				else
				{
					node.parentNode = fromNode;
					node.state = Node.NodeState.Open;
					
					Vector3 nodePos = node.marker.transform.position;
					node.G = fromNode.G + Vector3.Distance(fromNodePos, nodePos);
					node.H = Vector3.Distance(nodePos, endPos);
					result.Add(node);
				}
			}
			return result;
		}
		
		private List<Node> GetConnectedNodes(Node startNode, Vector3 startPos, Vector3 endPos)
		{
			List<Node> tempNodes = TurnConnectedMarkersIntoNodes(startNode.marker, startPos, endPos);

			List<Node> nextNodes = GetAvailableConnectedNodes(startNode, tempNodes, endPos);
			return nextNodes;
		}

		public List<PathfindingMarker> GetPath()
		{
			// This will perform really badly, but it should work.
			// The problem is that the states and other values don't ever reset, so "closed" will always be "closed" even in new checks.
			// Ideally I would probably create a completely separate list for each 'instance' of the PathFinder (and also have separate PathFinders for each agent looking for a path, and have it run on their own separate threads)
			
			// DONE: Refactor pathfinder to run in parallel, and give each Agent their own instance of the Pathfinder, instead of on the Grid << Completely usurped this file
			
			ResetDictionary();
			
			Vector3 startPos = startMarker.transform.position;
			Vector3 endPos = endMarker.transform.position;
			float startDistToEnd = Vector3.Distance(startPos, endPos);
			
			Node startNode = GetOrAddMarkerToDictionary(startMarker, 0, startDistToEnd);
			Node endNode = GetOrAddMarkerToDictionary(endMarker, startDistToEnd, 0);
			
			bool success = Search(startNode, endNode, startPos, endPos);

			var path = new List<PathfindingMarker>();

			if (!success)
			{
				Logging.Log($"Failed to move from {startMarker.name} to {endMarker.name}");
				return path;
			}
			
			Node node = endNode;
			while (node.parentNode != null)
			{
				path.Add(node.marker);
				node = node.parentNode;
			}
			
			// Because the startNode will not have a 'parent node', you have to end by adding the parentNode to the end of the list
			if (node == startNode) path.Add(node.marker);
			
			path.Reverse();
				
			Logging.Log($"Moving from {startMarker.name} to {endMarker.name} uses the path:");
			path.ForEach(marker =>
			{
				Logging.Log(marker.name);
			});

			return path;
		}

		private bool Search(Node currNode, Node endNode, Vector3 startPos, Vector3 endPos)
		{
			currNode.state = Node.NodeState.Closed;

			List<Node> nextNodes = GetConnectedNodes(currNode, startPos, endPos);
			
			nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
			foreach (Node nextNode in nextNodes)
			{
				// If it found the end, return true.
				if (nextNode == endNode) return true;
				
				// If the next search returns true (meaning somewhere down the recursion the end has been found), return true.
				// If it returns false, then try again on the next node in the list.
				if (Search(nextNode, endNode, startPos, endPos)) return true;
			}
			// If the list is empty (Meaning 'currNode' has no non-closed adjacent nodes, return false
			return false;
		}

		public List<Pathfinding.Node> OpenList { get; set; }
	}
}