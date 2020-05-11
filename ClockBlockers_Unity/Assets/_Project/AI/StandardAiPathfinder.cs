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
			CurrentPathfinder?.End();
			pathfindingManager.RequestPath(this, transform.position, destination, highestReachableRelativeAltitude);
		}
	}
}