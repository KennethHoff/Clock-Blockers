using System;
using System.Diagnostics.CodeAnalysis;

using ClockBlockers.Input;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.StateMachines;
using ClockBlockers.StateMachines.Conditions;
using ClockBlockers.StateMachines.States;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


// TODO: Create a new type of StateMachine - one that does all the transitions themselves

// https://www.youtube.com/watch?v=V75hgcsCGOM

namespace ClockBlockers.AI.AiControllers
{
	[BurstCompile]
	public abstract class AiController : MonoBehaviour
	{

		public PlayerInputController controllingPlayer;

		[SerializeField]
		private StateMachine stateMachine;
		
		[NonSerialized]
		public IntervalReplayRunner replayRunner;
		
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
		
		
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		private void SetupStates()
		{
			var state_Idle = new Idle();
			var state_ReplicatingActions = new ReplicatingActions(); // What to inject..
			var state_Unlinked = new Unlinked();
			var state_ExternalControl = new ExternalControl(aiPathfinder);
			
			// var cond_outOfActions = new OutOfActions(replayRunner);
			// var cond_outOfTranslationData = new OutOfTranslationData(replayRunner);
			
			var cond_OutOfThingsToDo = new OutOfThingsToDo(replayRunner);
			var cond_ControlLost = new ControlLost(this);
			var cond_ControlRegained = new ControlRegained(this);
			
			AddTransition(state_ReplicatingActions, state_Unlinked, cond_OutOfThingsToDo);
			
			AddTransition(state_ExternalControl, state_Idle, cond_ControlRegained);
			
			AddAnyTransition(state_ExternalControl, cond_ControlLost);
		}

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

		private void AddTransition(IState from, IState to, ICondition condition)
		{
			stateMachine.AddTransition(from, to, condition);
		}

		private void AddAnyTransition(IState to, ICondition condition)
		{
			stateMachine.AddAnyTransition(to, condition);
		}

		private void Start()
		{
			Begin();
		}

		private void Update()
		{
			stateMachine.Tick();
		}

		public abstract void Begin();

		public abstract void End();
	}
}