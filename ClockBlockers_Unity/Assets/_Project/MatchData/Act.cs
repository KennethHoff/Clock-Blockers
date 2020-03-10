using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.ReplaySystem;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.Utility;

using Sisus.OdinSerializer.Utilities;

using UnityEngine;
using UnityEngine.Events;


namespace ClockBlockers.MatchData
{
	// TODO: Find a way to store the CharacterStorage across acts.
	public class Act : MonoBehaviour
	{
		[NonSerialized]
		public Round round;

		// List of all Clones to spawn - List<IReplayStorage> perhaps

		// List of all players to spawn - List <Player> perhaps, although I don't want the same player instance to be created.
		// Mostly for the independence of being able to create a completely random character and it working correctly
		[SerializeField]
		private UnityEvent ActCreatedEvent;

		[SerializeField]
		private UnityEvent ActBegunEvent;

		[SerializeField]
		private UnityEvent ActEndedEvent;

		[SerializeField]
		private UnityEvent ActRemovedEvent;

		private float actDuration;

		[SerializeField]
		private FloatReference timeWhenActStarted;
		
		
		private List<Character> playerCharacters;

		[NonSerialized]
		public List<IntervalReplayStorage> replaysCreated;

		public List<IntervalReplayStorage> replaysForThisAct;
		
		private bool _begun;

		// private bool Ongoing => _begun && !_ended;

		private bool _ended;


		public void Setup()
		{
			
			playerCharacters = new List<Character>();
			SpawnAllCharacters();

			_begun = false;
			_ended = false;


			ActCreatedEvent.Invoke();
		}

		public void Begin()
		{
			timeWhenActStarted.Value = Time.time;
			_begun = true;


			ActBegunEvent.Invoke();
		}


		public void End()
		{
			Logging.Log("Act ended!", this);
			_ended = true;


			ActEndedEvent.Invoke();
		}

		public void Remove()
		{
			gameObject.SetActive(false);
			
			replaysCreated = new List<IntervalReplayStorage>();

			playerCharacters.ForEach(p => replaysCreated.Add(p.GetComponent<IntervalReplayStorage>()));

			ActRemovedEvent.Invoke();
		}

		private void SpawnAllCharacters()
		{
			SpawnAllPlayers();

			SpawnAllClones();
		}

		private void SpawnAllPlayers()
		{
			// TODO: For each player...
			SpawnNewPlayer();

			Logging.Log("Spawned all players on me!", this);
		}

		private void SpawnAllClones()
		{
			foreach (IntervalReplayStorage replayStorage in replaysForThisAct)
			{
				Character clone = SpawnNewClone();
				// clone.GetComponent<IntervalReplayRunner>().replays = replayStorage.PlayerActions;

				var cloneRunner = clone.GetComponent<IntervalReplayRunner>();
				
				cloneRunner.actions = replayStorage.actions;
				cloneRunner.translations = replayStorage.translations;

			}

			// TODO: For each clone...
			// SpawnNewClone();

			Logging.Log("Spawned all clones on me!", this);
		}


		[ContextMenu("Spawn a new Player")]
		public Character SpawnNewPlayer()
		{
			Character newPlayer = round.match.spawner.SpawnPlayer();

			newPlayer.transform.SetParent(transform, true);
			playerCharacters.Add(newPlayer);
			return newPlayer;
		}

		[ContextMenu("Spawn a new Clone")]
		public Character SpawnNewClone()
		{
			Character newClone = round.match.spawner.SpawnClone();

			newClone.transform.SetParent(transform, true);

			return newClone;
		}
	}
}