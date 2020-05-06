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
	internal class AStarPathFinder : MonoBehaviour, IPathfinder
	{
		private List<Node> TurnConnectedMarkersIntoNodes(PathfindingMarker startMarker, Vector3 startPos, Vector3 endPos)
		{
			var result = new List<Node>(startMarker.ConnMarkerCount);

			startMarker.connectedMarkers.Where(currMarker => currMarker != null).ForEach(connMarker =>
			{
				Vector3 currMarkerPos = connMarker.transform.position;
				
				float currDistToStart = Vector3.Distance(currMarkerPos, startPos);
				float currDistToEnd = Vector3.Distance(currMarkerPos, endPos);

				Node currNode = Grid.GetOrAddMarkerToDictionary(connMarker, currDistToStart, currDistToEnd);
				// Node node = Grid.GetOrAddMarkerToDictionary(connMarker);

				result.Add(currNode);
			});
			return result;
		}
		
		private List<Node> GetAvailableConnectedNodes(Node fromNode, List<Node> tempNodes)
		{
			var result = new List<Node>();
			
			foreach (Node node in tempNodes)
			{
				// If it's closed, that means it has been in a path at some point (Current path, or rejected). If it's in the current path, then obviously don't check it, and if it's not in the current path, that means the rejected path is worse.
				if (node.state == Node.NodeState.Closed) continue;

				// If it's open, that means it's open for consideration; Only added if the new path is shorter than the previous
				if (node.state == Node.NodeState.Open)
				{
					float gTemp = fromNode.G + Vector3.Distance(fromNode.marker.transform.position, node.marker.transform.position);
					if (gTemp >= node.G) continue;
					
					node.parentNode = fromNode;
					result.Add(node);
				}
				// If it hasn't been checked before, that means the new path has to be shorter than the previous.
				else
				{
					node.parentNode = fromNode;
					node.state = Node.NodeState.Open;
					result.Add(node);
				}
			}
			return result;
		}
		
		private List<Node> GetConnectedNodes(Node startNode, Vector3 startPos, Vector3 endPos)
		{
			List<Node> tempNodes = TurnConnectedMarkersIntoNodes(startNode.marker, startPos, endPos);

			List<Node> nextNodes = GetAvailableConnectedNodes(startNode, tempNodes);
			return nextNodes;
		}

		public List<PathfindingMarker> GetPath(PathfindingMarker startMarker, PathfindingMarker endMarker)
		{
			// This will perform really badly, but it should work.
			// The problem is that the states and other values don't ever reset, so "closed" will always be "closed" even in new checks.
			// Ideally I would probably create a completely separate list for each 'instance' of the PathFinder (and also have separate PathFinders for each agent looking for a path, and have it run on their own separate threads)
			// TODO: Refactor pathfinder to run in parallel, and give each Agent their own instance of the Pathfinder, instead of on the Grid
			
			
			Grid.ResetDictionary();
			
			Vector3 startPos = startMarker.transform.position;
			Vector3 endPos = endMarker.transform.position;
			float startDistToEnd = Vector3.Distance(startPos, endPos);
			
			Node startNode = Grid.GetOrAddMarkerToDictionary(startMarker, 0, startDistToEnd);
			Node endNode = Grid.GetOrAddMarkerToDictionary(endMarker, startDistToEnd, 0);
			
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
			List<Node> nextNodes = GetConnectedNodes(currNode, startPos, endPos);

			currNode.state = Node.NodeState.Closed;

			nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
			foreach (Node nextNode in nextNodes)
			{
				if (nextNode == endNode) return true;
				else
				{
					if (Search(nextNode, endNode, startPos, endPos)) return true;
				}
			}
			return false;
		}

		public PathfindingGrid Grid { get; set; }
	}
}