using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using UnityEngine;
using UnityEngine.Events;


namespace ClockBlockers.MatchData {
	public class Round : MonoBehaviour
	{

		[SerializeField]
		private UnityEvent roundCreatedEvent;
		
		[SerializeField]
		private UnityEvent roundBegunEvent;

		[SerializeField]
		private UnityEvent roundEndedEvent;

		[SerializeField]
		private UnityEvent roundRemovedEvent;
		
		
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
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref actPrefab, this, "Act Prefab");
		}

		public void Setup()
		{
			StartNewAct();

			roundCreatedEvent.Invoke();
		}

		public void Begin()
		{
			Logging.Log("Round ended", this);
			
			
			roundBegunEvent.Invoke();
		}
		
		public void End()
		{
			// Look at leaderboard etc..
			
			
			roundEndedEvent.Invoke();
		}

		private Act PreviousAct()
		{
			return allActs.Last();
		}

		public void Remove()
		{
			gameObject.SetActive(false);
			
			roundRemovedEvent.Invoke();
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
