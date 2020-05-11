using System;

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
			const float timeBeforeMarkerToChangeTargetMarker = 0.2f;
			float distanceBeforeMarkerToChangeTargetMarker = characterMovement.MoveSpd * timeBeforeMarkerToChangeTargetMarker;
			
			float distanceToCurrentPathMarker = DistanceToCurrentPathMarker();

			if (distanceToCurrentPathMarker < distanceBeforeMarkerToChangeTargetMarker)
			{
				if (currentPath.Count == 0)
				{
					Logging.Log("Reached the final marker!");
					currentPath = null;
					return;
				}
				
				GetNextMarkerInPath();
			}

			Vector3 currentMarkerPos = currentMarker.transform.position;
			
			Vector3 direction = (currentMarkerPos - transform.position).normalized;
			
			characterMovement.SetForwardInputVelocity(direction);
		}

		public override void Tick()
		{
			if (currentPath == null) return;

			MoveTowardsNextWaypoint();
		}

		public override void RequestPath(Vector3 destination)
		{
			CurrentPathfinder?.End();
			pathfindingManager.RequestPath(this, transform.position, destination, highestReachableRelativeAltitude);
		}
	}
}