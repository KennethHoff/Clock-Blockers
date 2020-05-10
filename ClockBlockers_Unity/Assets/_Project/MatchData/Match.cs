using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Events;
using ClockBlockers.GameControllers;
using ClockBlockers.MapData;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace ClockBlockers.MatchData
{
	/// <summary>
	/// <para>You could create a new variant of this MonoBehaviour* for each variant of map you'd like. <para/> 
	/// For example how in CS:GO Dust2 has a map and a set of character models, while Cache has another map and a different set of character models. This would be the equivalent</para>
	/// <para>*Although, now that I say it, that sounds more like a ScriptableObject kind of thing, which I would like to change this to, but I digress</para>
	/// </summary>
	
	[BurstCompile]
	public class Match : MonoBehaviour
	{
		[Header("Game Events")]
		[SerializeField]
		private GameEvent matchCreatedEvent = null;
		
		[SerializeField]
		private GameEvent matchBegunEvent = null;

		[SerializeField]
		private GameEvent matchEndedEvent = null;

		[SerializeField]
		private GameEvent matchRemovedEvent = null;

		[Header("Setup")]
		[SerializeField]
		private Round roundPrefab = null;

		[SerializeField]
		private string battleArena = null;

		
		[Header("Instance Data")]
		[SerializeField]
		private List<Round> allRounds;
		
		
		[NonSerialized]
		public CharacterSpawner spawner;

		public int RoundNumber => allRounds.Count;
		public Round CurrentRound => allRounds.Last();
		
		private PathfindingGrid grid;

		private void Awake()
		{
			spawner = GetComponent<CharacterSpawner>();
			
			allRounds = new List<Round>();
		}

		public void Setup()
		{
			SceneManager.LoadScene(battleArena, LoadSceneMode.Additive);
			
			grid = FindObjectOfType<PathfindingGrid>();
			
			spawner.grid = grid;

			matchCreatedEvent.Raise();
			
			StartCoroutine(BeginNextFrame());
		}

		private IEnumerator BeginNextFrame()
		{
			yield return null;
			Begin();
		}

		public void Begin()
		{
			StartNewRound();
			
			
			matchBegunEvent.Raise();
		}

		public void End()
		{
			Logging.Log("Match ended!", this);
			
			
			matchEndedEvent.Raise();
		}

		public void Remove()
		{
			// Unload the scene etc..

			gameObject.SetActive(false);
			
			matchRemovedEvent.Raise();
		}

		private void StartNewRound()
		{
			Round newRound = Instantiate(roundPrefab, transform, true);
			newRound.match = this;

			allRounds.Add(newRound);
			
			newRound.Setup();
			
			// newRound.Begin();
		}
		
		public void StopMatch()
		{
			
		}
	}
}