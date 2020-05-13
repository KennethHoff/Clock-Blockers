using System;
using System.Collections.Generic;

using Between_Names.Property_References;

using ClockBlockers.GameControllers;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayStorage
{
	// This Class's jobs are:
	
	// Store and retrieve CharacterActions (A combination of action, parameter for said action, and time when to execute said action)
	
	[BurstCompile]
	public class ActionReplayStorage : MonoBehaviour
	{
		[SerializeField]
		private FloatReference timeWhenActStarted = null;
		
		[SerializeField]
		private bool logEveryAction = false;

		[SerializeField]
		private bool saveActions = false;

		private GameController _gameController;

		private List<CharacterAction> PlayerActions { get; set; }

		private List<Tuple<Actions, float[]>> _currentFrameCharacterActions;

		private void Awake()
		{
			PlayerActions = new List<CharacterAction>();
			_gameController = FindObjectOfType<GameController>();
		}

		private void Start()
		{
			Logging.CheckIfCorrectMonoBehaviourInstantiation(_gameController, this, "Game Controller");
			_currentFrameCharacterActions = new List<Tuple<Actions, float[]>>();
		}

		private void FixedUpdate()
		{
			foreach ((Actions actions, float[] parameters) in _currentFrameCharacterActions)
			{
				var newCharacterAction = new CharacterAction(actions, parameters, Time.time - timeWhenActStarted);
				
				PlayerActions.Add(newCharacterAction);
				
				if (logEveryAction)
				{
					Logging.Log(newCharacterAction, this);
				}
			}
			_currentFrameCharacterActions.Clear();

		}

		// ReSharper disable once MemberCanBePrivate.Global
		public void SaveAction(Actions action, float[] parameter)
		{
			if (!saveActions) return;
			_currentFrameCharacterActions.Add(new Tuple<Actions, float[]>(action, parameter));
		}

		public void SaveAction(Actions action, float parameter)
		{
			SaveAction(action, new[] {parameter});
		}

		public void SaveAction(Actions action)
		{
			SaveAction(action, new float[0]);
		}
	}
}