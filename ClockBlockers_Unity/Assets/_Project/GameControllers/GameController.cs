using System.Collections.Generic;

using ClockBlockers.MatchData;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.GameControllers
{
	// TODO: Completely remove this Class. What the fuck is a "Game Controller" - this entire thing is a game..

	// This class' jobs are: [as of 27.02.2020]
	
	// Store the hierarchy location of the clones.
		// NEW CharacterCreator.
	
	
	// Fixed the following:
	
	
	// [26.02.2020]: Store the bullet hole prefab (dafuq, why??)
		// On the Gun itself.
		
	// [26.02.2020]: Know when an Act has started, and how long it has been running.
		// NEW classes: Act, Round, and Match
		
	// [27.02.2020]: Set Cursor state
		// On the InputController.
		
	// [27.02.2020]: Decide the floating point precision of storing of Actions
		// Removed. 
		
	// [27.02.2020]: Spawn new Characters. (Clones and Players)
		// On the Act (For now?).
		// Moved again to CharacterSpawner. I don't love that, but it'll do for now.
		
	// Load the map
		// On the new 'Match' class.
			// It does make sense for a map to be loaded by the 'latest thing' in the sequence that is guaranteed to never despawn. A match will always be on a single map.


	public class GameController : MonoBehaviour
	{
		
		[SerializeField]
		private List<Match> allMatches;
		
		[SerializeField]
		private bool logToFile;
		
		[SerializeField]
		private Match matchPrefab;
		
		
		private Match _currentMatch;
		
		private Logging _logging;

		private void Awake()
		{
			_logging = new Logging("Logging", logToFile);
		}

		private void Start()
		{
			Match newMatch = Instantiate(matchPrefab);
			allMatches.Add(newMatch);
			
			newMatch.Setup();
			newMatch.Begin();
			Application.targetFrameRate = 60;
		}
	}
}