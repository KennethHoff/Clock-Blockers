using System.Diagnostics.CodeAnalysis;

using ClockBlockers.StateMachines;
using ClockBlockers.StateMachines.Conditions;
using ClockBlockers.StateMachines.States;

using Unity.Burst;


namespace ClockBlockers.AI.AiControllers
{
	[BurstCompile]
	public class StandardAiController : AiController
	{
		private IState _idle;
		private IState _replicatingActions;
		private IState _unlinked;
		private IState _externalControl;
		
		// private IState _waitingForRoundToBegin;

		
		private ICondition _outOfThingsToDo;
		private ICondition _controlLost;
		private ICondition _controlRegained;
		private ICondition _roundBegun;
		private ICondition _hasThingsToDo;

		[SuppressMessage("ReSharper", "InconsistentNaming")]
		protected override void SetupStates()
		{
			_idle = new Idle();
			_replicatingActions = new ReplicatingActions(aiPathfinder, replayRunner);
			_unlinked = new Unlinked();
			_externalControl = new ExternalControl(aiPathfinder);
			
			// _waitingForRoundToBegin = new WaitingForRoundToBegin();
		
			
			_outOfThingsToDo = new OutOfThingsToDo(replayRunner);
			_controlLost = new ControlLost(this);
			_controlRegained = new ControlRegained(this);
			_hasThingsToDo = new HasThingsToDo(replayRunner);

			// _roundBegun = new RoundBegun();
			
			
			AddTransition(_replicatingActions, _unlinked, _outOfThingsToDo);
			
			AddTransition(_idle, _replicatingActions, _hasThingsToDo);
			
			AddTransition(_externalControl, _idle, _controlRegained);

			// AddTransition(_waitingForRoundToBegin, _idle, _roundBegun);

			
			AddAnyTransition(_externalControl, _controlLost);
		}

		protected override void Begin()
		{
			stateMachine.Initialize(_idle);
		}
	}
}