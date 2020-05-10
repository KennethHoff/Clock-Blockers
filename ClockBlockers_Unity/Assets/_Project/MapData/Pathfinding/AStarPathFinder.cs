using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;

namespace ClockBlockers.MapData.Pathfinding
{
	// https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2

	// TODO: Remove the entire dictionary. It feels unnecessary.

	[BurstCompile]
	public class AStarPathFinder : IPathfinder
	{
		private readonly PathRequest pathRequest;

		private readonly int checksPerFrame;

		private int totalChecks;
		private int framesTaken;


		private readonly List<PathfindingMarker> path;
		private List<PathfindingMarker> Path => path.Count > 0 ? path : null;
		
		private Dictionary<int, Node> markerNodeDictionary;

		public List<Node> OpenList { get; set; }

		private readonly List<Node> closedList;

		public static AStarPathFinder CreateInstance(PathRequest pathRequest, int checksPerFrame)
		{
			return new AStarPathFinder(pathRequest, checksPerFrame);
		}
		
		private AStarPathFinder(PathRequest pathRequest, int checksPerFrame)
		{
			this.pathRequest = pathRequest;
			
			this.checksPerFrame = checksPerFrame;

			totalChecks = 0;
			
			path = new List<PathfindingMarker>();
			
			markerNodeDictionary = new Dictionary<int, Node>();

			OpenList = new List<Node>();
			closedList = new List<Node>();
			
			Logging.Log("New instance of AStarPathFinder was constructed!");
		}
		~AStarPathFinder()
		{
			Logging.Log("An instance of AStarPathFinder was deconstructed!");
		}
		
		// ReSharper disable once SuggestBaseTypeForParameter
		private void AddToMarkerNodeDictionary(PathfindingMarker marker, Node node)
		{
			markerNodeDictionary.Add(marker.GetInstanceID(), node);
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
		
		// ReSharper disable once SuggestBaseTypeForParameter
		private Node GetNodeFromMarker(PathfindingMarker marker)
		{
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
		
		private IEnumerable<Node> TurnAvailableConnectedMarkersIntoNodes(PathfindingMarker marker)
		{
			IEnumerable<PathfindingMarker> availableConnectedMarkers = marker.GetAvailableConnectedMarkers(pathRequest.maxJumpHeight);
			if (availableConnectedMarkers == null) return null;
			return from currMarker in availableConnectedMarkers
				select GetOrAddMarkerToDictionary(currMarker);
		}
		public IEnumerator FindPathCoroutine()
		{
			ResetDictionary();

			Node startNode = GetOrAddMarkerToDictionary(pathRequest.startMarker);
			Node endNode = GetOrAddMarkerToDictionary(pathRequest.endMarker);

			float distFromStartToEnd = Vector3.Distance(pathRequest.startMarker.transform.position, pathRequest.endMarker.transform.position);

			startNode.G = 0;
			startNode.H = distFromStartToEnd;

			endNode.G = distFromStartToEnd;
			endNode.H = 0;

			OpenList.Add(startNode);

			while (OpenList.Count > 0)
			{
				totalChecks++;
				
				if (totalChecks % checksPerFrame == 0 )
				{
					framesTaken++;
					yield return null;
				}

				if (OpenList.Count > 1)
				{
					OpenList.Sort((node1, node2) => node1.F.CompareTo(node2.F));
				}

				Node currNode = OpenList.First();

				OpenList.Remove(currNode);
				closedList.Add(currNode);

				if (currNode == endNode)
				{
					Node current = currNode;

					while (current != null)
					{
						path.Add(current.marker);
						current = current.parentNode;
					}

					path.Reverse();
					break;
				}

				Vector3 currMarkerPos = currNode.marker.transform.position;

				currNode.childNodes = TurnAvailableConnectedMarkersIntoNodes(currNode.marker);

				if (currNode.childNodes == null) continue;
				
				foreach (Node childNode in currNode.childNodes)
				{
					if (closedList.Contains(childNode)) continue;

					Vector3 childMarkerPos = childNode.marker.transform.position;

					// TODO: Redo this into something .. better (Currently it randomly goes diagonally back and forth, since that's equally "good")
					float tempG = currNode.G + Vector3.Distance(childMarkerPos, currMarkerPos);

					float tempH = Vector3.Distance(childMarkerPos, endNode.marker.transform.position).Round(4);

					if (OpenList.Contains(childNode))
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

					OpenList.Add(childNode);
				}
			}
			
			// End of search

			Logging.Log($"Pathfinding completed {(path.Count > 0 ? "Successfully" : "Unsuccessfully" )}. Checked {totalChecks} markers for {pathRequest.pathRequester}. It took {framesTaken+1} frames to complete.");
			
			if (pathRequest.pathRequester == null) yield break;
			
			pathRequest.pathRequester.PathCallback(Path);
			
			pathRequest.pathRequester.CurrentPathfinder = null;
		}
	}
}