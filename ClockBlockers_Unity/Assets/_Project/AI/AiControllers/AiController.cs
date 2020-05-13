using System;

using ClockBlockers.Input;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.StateMachines;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


// https://www.youtube.com/watch?v=V75hgcsCGOM

namespace ClockBlockers.AI.AiControllers
{
	[BurstCompile]
	public abstract class AiController : MonoBehaviour
	{

		public PlayerInputController controllingPlayer;

		[SerializeField]
		protected StateMachine stateMachine;
		
		protected IntervalReplayRunner replayRunner;
		
		[NonSerialized]
		public AiPathfinder aiPathfinder;

		private void Awake()
		{
			replayRunner = GetComponent<IntervalReplayRunner>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref replayRunner, this, "Replay Runner");

			aiPathfinder = GetComponent<AiPathfinder>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref aiPathfinder, this, "Ai Pathfinder");
			
			InitializeStateMachine();
		}

		private void InitializeStateMachine()
		{
			stateMachine = new StateMachine();

			SetupStates();
		}
		

		// TODO: Turn states and conditions into ScriptableObjects. [More inside]
		// You input a 'State' ScriptableObject and a 'Condition' Scriptable Object in the Inspector.

		protected abstract void SetupStates();

		protected abstract void Begin();


		public void TakeControl(PlayerInputController player)
		{
			controllingPlayer = player;
		}

		public void WithdrawControl(PlayerInputController player)
		{
			if (controllingPlayer == player)
			{
				controllingPlayer = null;
			}
		}
		protected void AddTransition(IState from, IState to, ICondition condition)
		{
			stateMachine.AddTransition(from, to, condition);
		}

		protected void AddAnyTransition(IState to, ICondition condition)
		{
			stateMachine.AddAnyTransition(to, condition);
		}

		private void Update()
		{
			stateMachine.Tick();
		}
	}
}