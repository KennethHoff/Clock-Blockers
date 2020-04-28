using System;

using ClockBlockers.AI.States;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.AI.AiControllers
{
	public abstract class AiController : MonoBehaviour
	{
		public AiState currentState;

		public String CurrentStateString => currentState.GetType().Name;

		[NonSerialized]
		public ReplayRunner replayRunner;

		[NonSerialized]
		public AiPathfinder aiPathfinder;

		private void Awake()
		{
			replayRunner = GetComponent<ReplayRunner>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref replayRunner, this, "Replay Runner");

			aiPathfinder = GetComponent<AiPathfinder>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref aiPathfinder, this, "Ai Pathfinder");
		}

		private void Start()
		{
			Begin();
		}

		public void SetState(AiState newState)
		{
			Logging.Log(name + " is changing state to: " + newState.GetType().Name);

			currentState?.End();

			currentState = newState;


			StartCoroutine(currentState.Begin());
		}

		public abstract void Begin();

		public abstract void End();
	}
}