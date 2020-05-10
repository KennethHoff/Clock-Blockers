using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.AI
{
	[BurstCompile]
	public class StandardAiPathfinder : AiPathfinder
	{

		public override void MoveTowardsNextWaypoint()
		{
			Vector3 direction = (CurrentPathMarker.transform.position - transform.position).normalized;
			
			characterMovement.SetForwardInputVelocity(direction);
		}

		public override void RunPathfinding()
		{
			base.RunPathfinding();
			
			MoveTowardsNextWaypoint();
		}

		public override void RequestPath(Vector3 destination)
		{
			pathfindingManager.RequestPath(this, transform.position, destination, highestReachableRelativeAltitude);
		}
	}
}