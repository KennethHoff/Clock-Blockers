using System.Collections.Generic;
using System.Linq;


namespace ClockBlockers.Actions
{
	public class ActionStorage
	{
		internal ActionStorage(LinkedList<CharacterAction> newlyAddedActions = null, List<CharacterAction[]> gameActions = null, CharacterAction[] actionArray = null)
		{
			NewlyAddedActions = newlyAddedActions ?? new LinkedList<CharacterAction>();
			ReplayActions = actionArray ?? new CharacterAction[0];
			GameActions = gameActions ?? new List<CharacterAction[]>();
		}

		internal void AddActionToUpdatableList(CharacterAction action)
		{
			NewlyAddedActions.AddLast(action);
		}

		internal void ResetUpdatableList()
		{
			NewlyAddedActions.Clear();
		}

		internal void PushRoundDataToList()
		{
			GameActions.Add(NewlyAddedActions.ToArray());
		}


		internal LinkedList<CharacterAction> NewlyAddedActions { get; }

		internal List<CharacterAction[]> GameActions { get; set; }

		internal CharacterAction[] ReplayActions { get; }
	}
}