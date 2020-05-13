﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.MapData.Pathfinding.PathfindingManagement;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;

namespace ClockBlockers.MapData.Pathfinding
{
	// https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2

	[BurstCompile]
	public class AStarPathFinder : IPathfinder
	{
		private readonly PathRequest _pathRequest;

		private readonly int _checksPerFrame;

		private int _totalChecks;
		private int _framesTaken;

		private IPathfindingManager pathfindingManager;


		private readonly List<PathfindingMarker> _path;
		private List<PathfindingMarker> Path => _path.Count > 0 ? _path : null;
		
		private Dictionary<int, Node> _markerNodeDictionary;

		private int PathfinderIndex { get; set; }
		public List<Node> OpenList { get; set; }

		public void EndPreemptively()
		{
			//  The Coroutine is technically being ran from the Manager, and I can't find a way to stop a coroutine by sending in a reference to this GameObject.
			// I could make a List<IPathfinder>, and add the instance of this Type to that list on creation, and remove it on deletion, but it's fine for now..
			Logging.Log("A Pathfinder would've ended had I known how to... ");
		}
		
		private readonly List<Node> _closedList;

		public static AStarPathFinder CreateInstance(PathRequest pathRequest, int checksPerFrame, int pathfinderIndex)
		{
			return new AStarPathFinder(pathRequest, checksPerFrame, pathfinderIndex);
		}
		
		private AStarPathFinder(PathRequest pathRequest, int checksPerFrame, int pathfinderIndex)
		{
			_pathRequest = pathRequest;
			
			_checksPerFrame = checksPerFrame;

			_totalChecks = 0;
			
			_path = new List<PathfindingMarker>();
			
			_markerNodeDictionary = new Dictionary<int, Node>();

			OpenList = new List<Node>();
			_closedList = new List<Node>();

			PathfinderIndex = pathfinderIndex;
			
			Logging.Log("New instance of AStarPathFinder was constructed!");
		}
		~AStarPathFinder()
		{
			Logging.Log("An instance of AStarPathFinder was deconstructed!");
		}
		
		// ReSharper disable once SuggestBaseTypeForParameter
		private void AddToMarkerNodeDictionary(PathfindingMarker marker, Node node)
		{
			_markerNodeDictionary.Add(marker.GetInstanceID(), node);
		}
		
		private void ResetDictionary()
		{
			if (_markerNodeDictionary == null)
			{
				_markerNodeDictionary = new Dictionary<int, Node>();
				return;
			}
			_markerNodeDictionary.Clear();
		}
		
		// ReSharper disable once SuggestBaseTypeForParameter
		private Node GetNodeFromMarker(PathfindingMarker marker)
		{
			_markerNodeDictionary.TryGetValue(marker.GetInstanceID(), out Node node);
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
			IEnumerable<PathfindingMarker> availableConnectedMarkers = marker.GetAvailableConnectedMarkers(_pathRequest.maxJumpHeight);
			if (availableConnectedMarkers == null) return null;
			return from currMarker in availableConnectedMarkers
				select GetOrAddMarkerToDictionary(currMarker);
		}
		
		public IEnumerator FindPathCoroutine()
		{
			ResetDictionary();

			Node startNode = GetOrAddMarkerToDictionary(_pathRequest.startMarker);
			Node endNode = GetOrAddMarkerToDictionary(_pathRequest.endMarker);

			if (startNode == endNode)
			{
				_path.Add(startNode.marker);
				Complete();
				yield break;
			}
			
			float distFromStartToEnd = Vector3.Distance(_pathRequest.startMarker.transform.position, _pathRequest.endMarker.transform.position);

			startNode.G = 0;
			startNode.H = distFromStartToEnd;

			endNode.G = distFromStartToEnd;
			endNode.H = 0;

			OpenList.Add(startNode);

			while (OpenList.Count > 0)
			{
				_totalChecks++;
				
				if (_totalChecks % _checksPerFrame == 0 )
				{
					_framesTaken++;
					
					yield return null;
				}

				if (OpenList.Count > 1)
				{
					OpenList.Sort((node1, node2) => node1.F.CompareTo(node2.F));
				}

				Node currNode = OpenList.First();

				OpenList.Remove(currNode);
				_closedList.Add(currNode);

				if (currNode == endNode)
				{
					RetracePath(currNode);
					break;
				}

				Vector3 currMarkerPos = currNode.marker.transform.position;

				currNode.childNodes = TurnAvailableConnectedMarkersIntoNodes(currNode.marker);

				if (currNode.childNodes == null) continue;
				
				foreach (Node childNode in currNode.childNodes)
				{
					if (_closedList.Contains(childNode)) continue;

					Vector3 childMarkerPos = childNode.marker.transform.position;

					// LOW-PRIO: Redo this into something .. better (Currently it randomly goes diagonally back and forth, since that's equally "good")
					
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

			Complete();
			yield break;
		}

		private void Complete()
		{

			IPathRequester pathRequester = _pathRequest.pathRequester;

			Logging.Log($"Pathfinder #{PathfinderIndex} completed {(_path.Count > 0 ? "Successfully" : "Unsuccessfully")}. Checked {_totalChecks} markers for {pathRequester}. It took {_framesTaken + 1} frames to complete.");

			pathRequester.PathCallback(Path, PathfinderIndex);
		}

		private void RetracePath(Node currNode)
		{
			Node current = currNode;

			while (current != null)
			{
				_path.Add(current.marker);
				current = current.parentNode;
			}

			_path.Reverse();
		}
	}
}