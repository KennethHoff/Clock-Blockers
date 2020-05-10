using System;
using System.Collections.Generic;
using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.Events;
using ClockBlockers.MapData;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.MatchData
{
	// DONE: Find a way to store the CharacterStorage across acts. << Not needed. The act itself is stored in a 'Round', and you'll access the values directly
	[BurstCompile]
	public class Act : MonoBehaviour
	{
		[NonSerialized]
		public Round round;

		// List of all Clones to spawn - List<IReplayStorage> perhaps

		// List of all players to spawn - List <Player> perhaps, although I don't want the same player instance to be created.
		// Mostly for the independence of being able to create a completely random character and it working correctly
		[SerializeField]
		private GameEvent actCreatedEvent = null;

		
		[SerializeField]
		private GameEvent actRemovedEvent = null; // Temporary

		private float _actDuration;

		[SerializeField]
		private FloatReference timeWhenActStarted = null;
		
		
		private List<Character> _playerCharacters;

		[NonSerialized]
		public List<IntervalReplayStorage> replaysCreated;

		public List<IntervalReplayStorage> replaysForThisAct;
		public void Setup()
		{
			
			_playerCharacters = new List<Character>();
			SpawnAllCharacters();

			actCreatedEvent.Raise();
		}

		// in CS:GO terms: This is when the buy phase is, and you can't move or shoot

		public void Begin()
		{
			timeWhenActStarted.Value = Time.time;
		}


		// in CS:GO terms: This is when the Leaderboard pops up, and you can't interact
		public void End()
		{
			Logging.Log("Act ended!", this);

			
			// The following is temporary 
			Remove();
			actRemovedEvent.Raise();

		}

		public void Remove()
		{
			gameObject.SetActive(false);
			
			replaysCreated = new List<IntervalReplayStorage>();

			_playerCharacters.ForEach(character => replaysCreated.Add(character.GetComponent<IntervalReplayStorage>()));

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

				var cloneReplayRunner = clone.GetComponent<ReplayRunner>();
				
				cloneReplayRunner.actions = replayStorage.actions;
				cloneReplayRunner.translations = replayStorage.translations;

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
			_playerCharacters.Add(newPlayer);
			
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