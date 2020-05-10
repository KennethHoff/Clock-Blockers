using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Events;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.MatchData {
	[BurstCompile]
	public class Round : MonoBehaviour
	{
		[Header("Game Events")]

		[SerializeField]
		private GameEvent roundCreatedEvent = null;
		
		[SerializeField]
		private GameEvent roundBegunEvent = null;

		[SerializeField]
		private GameEvent roundEndedEvent = null;

		[SerializeField]
		private GameEvent roundRemovedEvent = null;
		
		
		[Header("Setup")]
		
		[SerializeField]
		private Act actPrefab;

		
		[NonSerialized]
		public Match match;
		
		[Header("Instance Data")]
		[SerializeField]
		private List<Act> allActs = new List<Act>();


		public Act CurrentAct => allActs.Last();

		public int ActNumber => allActs.Count;

		private void Awake()
		{
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref actPrefab, this, "Act Prefab");
		}

		public void Setup()
		{
			StartNewAct();

			roundCreatedEvent.Raise();
			
			StartCoroutine(BeginNextFrame());
		}

		private IEnumerator BeginNextFrame()
		{
			yield return null;
			Begin();
		}

		public void Begin()
		{
			Logging.Log("Round ended", this);
			
			
			roundBegunEvent.Raise();
		}
		
		public void End()
		{
			// Look at leaderboard etc..
			
			roundEndedEvent.Raise();
		}

		private Act PreviousAct()
		{
			return allActs.Last();
		}

		public void Remove()
		{
			gameObject.SetActive(false);
			
			roundRemovedEvent.Raise();
		}

		public void StartNewAct()
		{
			Act newAct = Instantiate(actPrefab, transform, true);
			
			newAct.round = this;

			if (allActs.Count > 0)
			{
				newAct.replaysForThisAct = PreviousAct().replaysCreated;
			}
			
			allActs.Add(newAct);
			
			newAct.Setup();
			
			// "10 seconds after start..." or whatever
			newAct.Begin();
		}
	}
}
