using System.Collections.Generic;

using ClockBlockers.AI;
using ClockBlockers.ReplaySystem.ReplayRunner;

using UnityEngine;


namespace ClockBlockers.StateMachines.AI.States
{
	internal class ReplicatingActions : IState
	{
		private readonly AiPathfinder _aiPathfinder;
		private readonly IntervalReplayRunner _replayRunner;

		public ReplicatingActions(AiPathfinder aiPathfinder, IntervalReplayRunner replayRunner)
		{
			_aiPathfinder = aiPathfinder;
			_replayRunner = replayRunner;
		}

		public void Tick()
		{
			_aiPathfinder.Tick();
		}

		public void OnEnter()
		{
			// TODO: Currently it does not respect the interval between the translations.
			// Say you move from A -> B over 10 seconds, but it is like 1 meter apart (You're holding a corner or whatever),
			// it will move directly from A -> B, and when it comes to B it'll stop because the path is complete.

			// My current suggestion would be to change the 'PathCallback()' method (Use the 'WorkInProgressPath' directly, basically)
			// Basically: Instead of doing as it does now, where it's waiting for all of the 'WorkInProgressPath' paths to be completed before merging them all into a single 'Path',
			// it should instead start the 'WIP Path #1', and *intervalTime* after starting that path, it should start 'WIP Path #2'.

			List<Vector3> positions = _replayRunner.CreatePositionListFromTranslations();

			_aiPathfinder.RequestMultiPath(positions);
		}

		public void OnExit()
		{
			_replayRunner.Unlinked = true;
		}
	}
}