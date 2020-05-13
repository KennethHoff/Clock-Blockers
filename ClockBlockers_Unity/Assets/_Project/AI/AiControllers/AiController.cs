using System;

using ClockBlockers.Characters;
using ClockBlockers.Input;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.StateMachines;
using ClockBlockers.StateMachines.Conditions;
using ClockBlockers.ToBeMoved;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


// https://www.youtube.com/watch?v=V75hgcsCGOM

namespace ClockBlockers.AI.AiControllers
{
	[BurstCompile]
	public abstract class AiController : MonoBehaviour
	{
		#region States and Conditions

		protected static ICondition always;

		#endregion

		public PlayerInputController controllingPlayer;

		[SerializeField]
		protected StateMachine stateMachine;
		
		protected IntervalReplayRunner replayRunner;
		
		[NonSerialized]
		public AiPathfinder aiPathfinder;

		[SerializeField]
		protected HealthComponent healthComponent;

		protected Character character;

		

		private void Awake()
		{
			character = GetComponent<Character>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(character, this, "Character");
			
			healthComponent = GetComponent<HealthComponent>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(healthComponent, this, "Health Component");
			
			replayRunner = GetComponent<IntervalReplayRunner>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(replayRunner, this, "Replay Runner");

			aiPathfinder = GetComponent<AiPathfinder>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(aiPathfinder, this, "Ai Pathfinder");
			
			InitializeStateMachine();
		}

		private void InitializeStateMachine()
		{
			stateMachine = new StateMachine();

			SetupStates();
		}
		

		// TODO: Turn states and conditions into ScriptableObjects. [More inside]
		// You input a 'State' ScriptableObject and a 'Condition' Scriptable Object in the Inspector.

		protected virtual void SetupStates()
		{
			if (always != null) return;

			always = new Always();
		}

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