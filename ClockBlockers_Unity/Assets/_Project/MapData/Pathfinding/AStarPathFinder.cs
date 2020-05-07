using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.MapData.Pathfinding
{
	// https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2


	[BurstCompile]
	public class AStarPathFinder : IPathfinder
	{

		private readonly IPathRequester pathRequester;
		private readonly PathfindingMarker startMarker;
		private readonly PathfindingMarker endMarker;

		private readonly int checksPerFrame;

		private int totalChecks;
		private int framesTaken;

		private List<PathfindingMarker> path;
		private List<PathfindingMarker> Path => path.Count > 0 ? path : null;
		
		private Dictionary<int, Node> markerNodeDictionary;

		public List<Node> OpenList { get; set; }

		private readonly List<Node> closedList;

		private bool isComplete;
		public bool IsComplete => isComplete;


		public static AStarPathFinder CreateInstance(PathRequest pathRequest, int checksPerFrame)
		{
			return new AStarPathFinder(pathRequest.pathRequester, pathRequest.startMarker, pathRequest.endMarker, checksPerFrame);
		}
		
		private AStarPathFinder(IPathRequester pathRequester, PathfindingMarker startMarker, PathfindingMarker endMarker, int checksPerFrame)
		{
			this.pathRequester = pathRequester;
			
			this.startMarker = startMarker;
			this.endMarker = endMarker;
			
			this.checksPerFrame = checksPerFrame;
			
			isComplete = false;

			totalChecks = 0;
			
			path = new List<PathfindingMarker>();
			
			markerNodeDictionary = new Dictionary<int, Node>();

			Logging.Log($"New instance of AStarPathFinder was constructed!");
			OpenList = new List<Node>();
			closedList = new List<Node>();
		}
		~AStarPathFinder()
		{
			Logging.Log($"An instance of AStarPathFinder was deconstructed!");
		}
		
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
		
		private IEnumerable<Node> TurnConnectedMarkersIntoNodes(PathfindingMarker marker)
		{
			return from currMarker in marker.connectedMarkers
				where currMarker != null
				select GetOrAddMarkerToDictionary(currMarker);
		}
		public IEnumerator FindPathCoroutine()
		{
			ResetDictionary();

			Node startNode = GetOrAddMarkerToDictionary(startMarker);
			Node endNode = GetOrAddMarkerToDictionary(endMarker);

			float distFromStartToEnd = Vector3.Distance(startMarker.transform.position, endMarker.transform.position);

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

				currNode.childNodes = TurnConnectedMarkersIntoNodes(currNode.marker);

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

			Logging.Log($"Checked {totalChecks}. It took {framesTaken} frames to complete.");


			isComplete = true;

			if (pathRequester == null) yield break;
			
			pathRequester.CurrentPathfinder = null;
			pathRequester.PathCallback(Path);

		}

	}
}