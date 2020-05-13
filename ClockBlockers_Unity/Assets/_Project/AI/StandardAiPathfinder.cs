using System.Collections.Generic;

using ClockBlockers.MapData;
using ClockBlockers.MapData.Pathfinding;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.AI
{
	[BurstCompile]
	public class StandardAiPathfinder : AiPathfinder
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
			pathfindingManager.RequestPath(this, transform.position, destination, highestReachableRelativeAltitude);
		}

		private void EndAllPathfinders()
		{
			foreach (IPathfinder currentPathfinder in CurrentPathfinders)
			{
				currentPathfinder.EndPreemptively();
			}
		}

		public override void RequestMultiPath(List<Vector3> listOfPoints)
		{
			pathfindingManager.RequestMultiPath(this, transform.position, listOfPoints, highestReachableRelativeAltitude);

			int pathfinderCount = listOfPoints.Count-1;
			_workInProgressPath = new List<PathfindingMarker>[pathfinderCount];
			CurrentPathfinders = new IPathfinder[pathfinderCount];

		}
	}
}