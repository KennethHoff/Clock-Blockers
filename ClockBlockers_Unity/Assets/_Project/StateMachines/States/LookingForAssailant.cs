using ClockBlockers.AI;
using ClockBlockers.Visualizations;

using UnityEngine;


namespace ClockBlockers.StateMachines.States
{
	internal class LookingForAssailant : IState
	{
		private readonly AiPathfinder _aiPathfinder;
		private readonly VisualizerBase _lookingForAssailantVisualizer;
		private readonly Transform _transform;
		
		public LookingForAssailant(AiPathfinder aiPathfinder, VisualizerBase lookingForAssailantVisualizer, Transform transform)
		{
			_aiPathfinder = aiPathfinder;
			_lookingForAssailantVisualizer = lookingForAssailantVisualizer;
			_transform = transform;
		}
		
		public void Tick() { }

		public void OnEnter()
		{
			Vector3 startPos = _transform.position;
			startPos.y += 1.5f;

			_lookingForAssailantVisualizer.Create(startPos, Quaternion.identity, _transform);
		}
		public void OnExit() { }
	}
}