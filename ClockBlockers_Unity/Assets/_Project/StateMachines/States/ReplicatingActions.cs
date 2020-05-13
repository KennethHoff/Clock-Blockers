using System.Collections.Generic;

using ClockBlockers.AI;
using ClockBlockers.ReplaySystem.ReplayRunner;

using UnityEngine;


namespace ClockBlockers.StateMachines.States
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
			List<Vector3> positions = _replayRunner.CreatePositionListFromTranslations();
			
			_aiPathfinder.RequestMultiPath(positions);
		}

		public void OnExit() { }
	}
}