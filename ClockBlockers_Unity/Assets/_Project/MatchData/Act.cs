using System;

using ClockBlockers.Characters;
using ClockBlockers.Utility;

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

		private float _timeWhenActStarted;
		public float TimeSinceActStarted => _timeWhenActStarted - Time.time;


		private bool _begun;

		// private bool Ongoing => _begun && !_ended;

		private bool _ended;


		public void Setup()
		{
			SpawnAllCharacters();

			_begun = false;
			_ended = false;


			ActCreatedEvent.Invoke();
		}

		public void Begin()
		{
			_timeWhenActStarted = Time.time;
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
			// TODO: For each clone...
			SpawnNewClone();

			Logging.Log("Spawned all clones on me!", this);
		}


		[ContextMenu("Spawn a new Player")]
		private void SpawnNewPlayer()
		{
			Player newPlayer = round.match.spawner.SpawnPlayer();

			newPlayer.transform.SetParent(transform, true);
		}

		[ContextMenu("Spawn a new Clone")]
		private void SpawnNewClone()
		{
			Clone newClone = round.match.spawner.SpawnClone();

			newClone.transform.SetParent(transform, true);
		}
	}
}