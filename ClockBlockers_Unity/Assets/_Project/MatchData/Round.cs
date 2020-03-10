using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.ReplaySystem.ReplayStorage;
using ClockBlockers.Utility;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


namespace ClockBlockers.MatchData {
	public class Round : MonoBehaviour
	{

		[SerializeField]
		private UnityEvent RoundCreatedEvent;
		
		[SerializeField]
		private UnityEvent RoundBegunEvent;

		[SerializeField]
		private UnityEvent RoundEndedEvent;

		[SerializeField]
		private UnityEvent RoundRemovedEvent;
		
		
		[NonSerialized]
		public Match match;
		[SerializeField]
		private List<Act> allActs = new List<Act>();


		public Act CurrentAct => allActs.Last();

		public int ActNumber => allActs.Count;

		[SerializeField]
		private Act actPrefab;

		private void Awake()
		{
			if (actPrefab == null) Logging.LogIncorrectInstantiation("Act Prefab", this);
		}

		public void Setup()
		{
			StartNewAct();

			RoundCreatedEvent.Invoke();
		}

		public void Begin()
		{
			Logging.Log("Round ended", this);
			
			
			RoundBegunEvent.Invoke();
		}
		
		public void End()
		{
			// Look at leaderboard etc..
			
			
			RoundEndedEvent.Invoke();
		}

		private Act PreviousAct()
		{
			return allActs.Last();
		}

		public void Remove()
		{
			gameObject.SetActive(false);
			
			
			RoundRemovedEvent.Invoke();
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
