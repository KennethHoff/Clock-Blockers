using System.Collections.Generic;

using ClockBlockers.ReplaySystem;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.AI
{
	public class StandardAiPathfinder : AiPathfinder
	{
		
		// The path to the 'final destination'
		private List<Vector3> waypoints;

		protected override void Awake()
		{
			base.Awake();
			waypoints = new List<Vector3>();
		}

		private int currentWaypointIndex = 0;
		
		private Vector3 NextWaypoint => waypoints[currentWaypointIndex];
		
		public override void ChangeDestination(Translation newDestination) 
		{
			base.ChangeDestination(newDestination);
			
			CreateWaypoints();
		}
		
		protected override void MoveTowardsNextWaypoint()
		{
			// Vector3 newPos = Vector3.SmoothDamp(_transform.position, NextWaypoint.position, ref currVelocity, TimeLeftUntilNextInterval);
			// _transform.position = newPos;

			Vector3 currPos = transform.position;
			Vector3 waypointPos = destination.position;

			if (Vector3.Distance(currPos, waypointPos) < characterMovement.MoveSpd * Time.deltaTime) return;

			Vector2 newDirection = new Vector2(waypointPos.x - currPos.x, waypointPos.z - currPos.z).normalized;

			characterMovement.AddVelocityFromInputVector(newDirection);


			// _characterMovement.RotateTo(nextTranslation.position.y);

			// _characterMovement.AddVelocityRelativeToForward();

		}
		
		private void CreateWaypoints()
		{
			waypoints.Clear();
			if (CheckIfCollisionBetweenCurrentPosAndDestination())
			{
				Logging.Log("Collision between current position and destination");
			}

			CreateValidWaypoints();
		}

		private void CreateValidWaypoints()
		{
			waypoints.Add(destination.position);
		}

		private bool CheckIfCollisionBetweenCurrentPosAndDestination()
		{
			return Physics.Linecast(transform.position, destination.position);
		}
	}
}