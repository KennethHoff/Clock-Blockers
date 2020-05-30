using Between_Names.Property_References;

using ClockBlockers.StateMachines;
using ClockBlockers.StateMachines.AI.Conditions;
using ClockBlockers.StateMachines.AI.States;
using ClockBlockers.Visualizations;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.AI.AiControllers
{
	[BurstCompile]
	internal class StandardAiController : BaseAiController
	{

		#region States and Conditions

		private IState _idle;
		private IState _unlinked;

		private IState _replicatingActions;
		private IState _externalControl;
		private IState _walkingAround;
		private IState _lookingForAssailant;
		private IState _assaultingTarget;
		private IState _dead;
		
		// private IState _waitingForRoundToBegin;

		
		private ICondition _outOfThingsToDo;
		private ICondition _controlLost;
		private ICondition _controlRegained;
		private ICondition _roundBegun;
		private ICondition _hasThingsToDo;

		private ICondition _assailantFound;
		private ICondition _underFire;
		private ICondition _enemySpotted;

		private ICondition _targetDied;
		private ICondition _targetFled;

		private ICondition _died;

		#endregion


		[SerializeField]
		private VisualizerBase lookingForAssailantVisualizer = null;

		[SerializeField]
		private FloatReference viewAngle;

		[SerializeField]
		private FloatReference viewRange;


		protected override void SetupStates()
		{
			base.SetupStates();
			
			Transform thisTransform = transform;


			_idle = new Idle();
			_replicatingActions = new ReplicatingActions(aiPathfinder, replayRunner);
			_unlinked = new Unlinked();
			_externalControl = new ExternalControl(aiPathfinder);
			
			_walkingAround = new WalkingAround(aiPathfinder);
			_lookingForAssailant = new LookingForAssailant(aiPathfinder, lookingForAssailantVisualizer, thisTransform);
			// _assaultingTarget = new AssaultingTarget();

			_dead = new Dead(character);

			// _waitingForRoundToBegin = new WaitingForRoundToBegin();


			_hasThingsToDo = new HasThingsToDo(replayRunner);
			_outOfThingsToDo = new OutOfThingsToDo(aiPathfinder);

			_controlRegained = new ControlRegained(this);
			_controlLost = new ControlLost(this);


			_enemySpotted = new EnemySpotted(character, thisTransform, viewAngle, viewRange, character.Act);
			_underFire = new UnderFire(healthComponent);
			
			// _assailantFound = new AssailantFound();
			
			// _targetDied = new TargetDied();
			_targetFled = new TargetFled(_enemySpotted);
			
			_died = new Died(healthComponent);

			// _roundBegun = new RoundBegun();


			AddTransition(_lookingForAssailant, _idle, _targetFled);
			
			
			
			AddTransition(_replicatingActions, _unlinked, _outOfThingsToDo);


			AddTransition(_unlinked, _idle, always);
			
			
			AddTransition(_idle, _replicatingActions, _hasThingsToDo);
			AddTransition(_idle, _lookingForAssailant, _underFire);
			
			// Temp; Should be 'AssaultingTarget'
			AddTransition(_idle, _lookingForAssailant, _enemySpotted);

			
			// AddTransition(_walkingAround, _assaultingTarget, _enemySpotted);
			// AddTransition(_walkingAround, _lookingForAssailant, _underFire);

			
			AddTransition(_externalControl, _idle, _controlRegained);
			

			// AddTransition(_lookingForAssailant, _assaultingTarget, _assailantFound);
			
			
			// AddTransition(_assaultingTarget, _idle, _targetDied);
			// AddTransition(_assaultingTarget, _idle, _targetFled);

			// AddTransition(_waitingForRoundToBegin, _idle, _roundBegun);

			AddAnyTransition(_dead, _died);
			AddAnyTransition(_externalControl, _controlLost);
		}

		protected override void Begin()
		{
			stateMachine.Initialize(_idle);
		}

		protected override void End() { }
	}
}