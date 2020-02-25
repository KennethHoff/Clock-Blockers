using System.Collections.Generic;
using System.Linq;

using UnityEngine;


namespace ClockBlockers.NewReplaySystem.ReplayStorage
{
	public class ActionReplayStorage : MonoBehaviour, IReplayStorage
	{
		
		internal LinkedList<CharacterAction> CurrentActPlayerActions { get; set; }

		internal List<CharacterAction[]> RoundActions { get; set; }

		internal CharacterAction[] CurrentActNpcActions { get; set; }

		// internal ActionReplayStorage(LinkedList<CharacterAction> newlyAddedActions = null, List<CharacterAction[]> gameActions = null, CharacterAction[] actionArray = null)
		// {
		// 	CurrentActPlayerActions = newlyAddedActions ?? new LinkedList<CharacterAction>();
		// 	ActActions = actionArray ?? new CharacterAction[0];
		// 	RoundActions = gameActions ?? new List<CharacterAction[]>();
		// }

		private void Awake()
		{
			CurrentActPlayerActions = new LinkedList<CharacterAction>();
			RoundActions = new List<CharacterAction[]>();
			CurrentActNpcActions = new CharacterAction[0];
		}

		internal void AddActionToAct(CharacterAction action)
		{
			CurrentActPlayerActions.AddLast(action);
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
			throw new System.NotImplementedException();
		}

		public CharacterAction[] GetNewestAct()
		{
			return RoundActions.Last();
		}

		public CharacterAction[] GetCurrentAct()
		{
			return CurrentActPlayerActions.ToArray();
		}

		public void SetActActions(CharacterAction[] actions)
		{
			CurrentActNpcActions = actions;
		}

		public List<CharacterAction[]> GetAllActs()
		{
			return RoundActions;
		}
	}
}