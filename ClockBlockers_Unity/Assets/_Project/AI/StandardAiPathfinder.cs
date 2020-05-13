using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.MapData;
using ClockBlockers.MapData.Pathfinding;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.AI
{
	[BurstCompile]
	internal class StandardAiPathfinder : AiPathfinder
	{

		public override void MoveTowardsNextWaypoint()
		{
			if (currentMarker == null) GetNextMarkerInPath();
			
			Vector3 currentMarkerPos = currentMarker.transform.position;

			LookAt(currentMarkerPos);

			characterMovement.SetVelocityForward();
			
			if (characterMovement.IsHittingASide)
			{
				characterMovement.Jump();
			}
			
			const float timeBeforeMarkerToChangeTargetMarker = 0.2f;
			float distanceBeforeMarkerToChangeTargetMarker = characterMovement.MoveSpd * timeBeforeMarkerToChangeTargetMarker;
			
			float distanceToCurrentPathMarker = HorizontalDistanceToCurrentPathMarker();


			if (distanceToCurrentPathMarker >= distanceBeforeMarkerToChangeTargetMarker) return;
			
			if (currentPath.Count == 0)
			{
				Logging.Log("Reached the final marker!");
				currentPath = null;
				hasCompletedAPath = true;
				return;
			}

			GetNextMarkerInPath();
		}

		private void LookAt(Vector3 point)
		{
			var targetPosition = new Vector3(point.x, transform.position.y, point.z);
			characterMovement.transform.LookAt(targetPosition);
		}

		public override void Tick()
		{
			if (currentPath == null) return;

			MoveTowardsNextWaypoint();
		}

		public override void RequestPath(Vector3 destination)
		{
			EndAllPathfinders();

			workInProgressPath = new List<PathfindingMarker>[1];
			CurrentPathfinders = new IPathfinder[1];
			pathfindingManager.RequestPath(this, transform.position, destination, highestReachableRelativeAltitude);
		}

		private void RequestPath(PathfindingMarker destinationMarker)
		{
			EndAllPathfinders();
			
			workInProgressPath = new List<PathfindingMarker>[1];
			CurrentPathfinders = new IPathfinder[1];
			
			pathfindingManager.RequestPath(this, transform.position, destinationMarker, highestReachableRelativeAltitude);
		}

		private void EndAllPathfinders()
		{
			if (CurrentPathfinders == null) return;
			foreach (IPathfinder currentPathfinder in CurrentPathfinders)
			{
				currentPathfinder.EndPreemptively();
			}
		}

		public override void RequestMultiPath(List<Vector3> listOfPoints)
		{
			int pathfinderCount = listOfPoints.Count-1;
			workInProgressPath = new List<PathfindingMarker>[pathfinderCount];
			CurrentPathfinders = new IPathfinder[pathfinderCount];
			
			pathfindingManager.RequestMultiPath(this, transform.position, listOfPoints, highestReachableRelativeAltitude);
		}

		public override PathfindingMarker RequestPathToRandomLocationWithinDistance(float distance)
		{
			// This is dumb (I get a random point, and then ask for a marker near that point, then get that marker's position and request a path to that position,
			// which in turn looks for the marker near that position)
			
			// Logically I would create a separate 'RequestPath' that takes in a PathfindingMarker (and that's easy),
			// BUT... I am fully planning on creating a separate non-marker-based pathfinding system at some point in the near-future,
			// therefore I don't want to create marker-based functions in the AiController directly. 
			
			
			PathfindingMarker randomMarker = grid.FindRandomMarkerWithinDistance(transform.position, distance);
			RequestPath(randomMarker.transform.position);
			return null;
		}
	}
}