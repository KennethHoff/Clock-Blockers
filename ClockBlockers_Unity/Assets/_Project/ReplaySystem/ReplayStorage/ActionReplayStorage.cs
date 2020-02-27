using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.DataStructures;
using ClockBlockers.GameControllers;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.ReplaySystem.ReplayStorage
{
	// This Class's jobs are:
	
	// Store and retrieve CharacterActions (A combination of action, parameter for said action, and time when to execute said action)
	
	public class ActionReplayStorage : MonoBehaviour, IReplayStorage
	{
		[SerializeField]
		private bool logEveryAction;

		[SerializeField]
		private bool saveActions;

		private GameController _gameController;

		private List<CharacterAction> CurrentActPlayerActions { get; set; }

		private List<CharacterAction[]> RoundActions { get; set; }

		internal CharacterAction[] CurrentActNpcActions { get; private set; }

		private List<Tuple<Actions, float[]>> _currentFrameCharacterActions;

		private void Awake()
		{
			// DILEMMA: I do not want use "FindObjectOfType" on GameController, because I don't want the singleton pattern, but I also can't assign GameController via the Interface.
			// This will hopefully be fixed by the time I refactor the GameController into smaller parts.
			
			CurrentActPlayerActions = new List<CharacterAction>();
			RoundActions = new List<CharacterAction[]>();
			CurrentActNpcActions = new CharacterAction[0];
			_gameController = FindObjectOfType<GameController>();
		}

		private void Start()
		{
			if (_gameController == null) Logging.instance.LogIncorrectInstantiation("Game Controller", this);
			_currentFrameCharacterActions = new List<Tuple<Actions, float[]>>();
		}

		private void FixedUpdate()
		{
			foreach ((Actions actions, float[] parameters) in _currentFrameCharacterActions)
			{
				// The previous time was based on incorrect information
				// var newCharacterAction = new CharacterAction(actions, parameters, _gameController.FixedTimeSinceActStarted);
				// CurrentActPlayerActions.Add(newCharacterAction);
				// if (logEveryAction)
				// {
					// Logging.instance.Log(newCharacterAction, this);
				// }

			}
			_currentFrameCharacterActions.Clear();

		}

		internal void AddActionToAct(CharacterAction action)
		{
			CurrentActPlayerActions.Add(action);
		}

		internal void ResetCurrentActPlayerActions()
		{
			CurrentActPlayerActions.Clear();
		}

		internal void PushActDataToRound()
		{
			RoundActions.Add(CurrentActPlayerActions.ToArray());
		}


		public void ClearStorageForThisAct()
		{
			CurrentActNpcActions = new CharacterAction[0];
			CurrentActPlayerActions = new List<CharacterAction>();
		}

		public CharacterAction[] GetNewestAct()
		{
			return RoundActions.Last();
		}

		//TODO: Separate replay and player storage?
		public CharacterAction[] GetCurrentAct()
		{
			return CurrentActNpcActions.Length > 0 
				? CurrentActNpcActions.ToArray() 
				: CurrentActPlayerActions.ToArray();
		}

		public void SetActReplayData(CharacterAction[] actions)
		{
			CurrentActNpcActions = actions;
		}

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

		public void StoreActData()
		{
			PushActDataToRound();
		}

		public List<CharacterAction[]> GetAllActs()
		{
			return RoundActions;
		}
	}
}