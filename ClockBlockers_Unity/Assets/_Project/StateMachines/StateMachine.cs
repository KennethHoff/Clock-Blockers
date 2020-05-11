using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;


namespace ClockBlockers.StateMachines
{
	// https://www.youtube.com/watch?v=V75hgcsCGOM
	public class StateMachine
	{
		private IState _current;
		
		private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>(); // From A => B/C/D, From B => A/C/F, from D => A/E/G, etc..
		private List<Transition> _currentTransitions = new List<Transition>(); // From currentState => ..

		private List<Transition> _anyTransitions = new List<Transition>(); // From any state => ... | These are generally more urgent ("Stop AI", "Give control to a player", etc..)

		// Basically a 'null'
		// ReSharper disable once FieldCanBeMadeReadOnly.Local
		private static List<Transition> EmptyTransitions = new List<Transition>(0);

		public void Tick()
		{
			Transition transition = GetTransition();
			if (transition != null)
			{
				SetState(transition.To);
			}

			_current?.Tick();
		}

		private void SetState(IState state)
		{
			if (state == _current) return;

			_current?.OnExit();
			_current = state;

			_transitions.TryGetValue(_current.GetType(), out _currentTransitions);

			if (_currentTransitions == null)
			{
				_currentTransitions = EmptyTransitions;
			}

			_current.OnEnter();
		}


		public void AddTransition(IState from, IState to, ICondition condition)
		{
			Type fromStateType = from.GetType();
			
			if (_transitions.TryGetValue(fromStateType, out List<Transition> transitions) == false)
			{
				transitions = new List<Transition>();
				_transitions[fromStateType] = transitions;
			}

			transitions.Add(new Transition(to, condition));
		}

		public void AddAnyTransition(IState state, ICondition condition)
		{
			_anyTransitions.Add(new Transition(state, condition));
		}


		private Transition GetTransition()
		{
			foreach (Transition transition in _anyTransitions.Where(transition => transition.Condition.Fulfilled()))
			{
				return transition;
			}

			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (Transition transition in _currentTransitions.Where(transition => transition.Condition.Fulfilled()))
			{
				return transition;
			}

			return null;
		}


		private class Transition
		{
			public ICondition Condition { get; }

			public IState To { get; }

			public Transition(IState to, ICondition condition)
			{
				To = to;
				Condition = condition;
			}
		}
	}
}