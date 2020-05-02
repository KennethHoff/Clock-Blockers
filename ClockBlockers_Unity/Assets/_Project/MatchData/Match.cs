using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.GameControllers;
using ClockBlockers.Utility;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace ClockBlockers.MatchData
{
	/// <summary>
	/// <para>You could create a new variant of this MonoBehaviour* for each variant of map you'd like. <para/> 
	/// For example how in CS:GO Dust2 has a map and a set of character models, while Cache has another map and a different set of character models. This would be the equivalent</para>
	/// <para>*Although, now that I say it, that sounds more like a ScriptableObject kind of thing, which I would like to change this to, but I digress</para>
	/// </summary>
	public class Match : MonoBehaviour
	{
		
		
		
		[SerializeField]
		private UnityEvent matchCreatedEvent = null;
		
		[SerializeField]
		private UnityEvent matchBegunEvent = null;

		[SerializeField]
		private UnityEvent matchEndedEvent = null;

		[SerializeField]
		private UnityEvent matchRemovedEvent = null;
		
		[SerializeField]
		private List<Round> allRounds;

		[NonSerialized]
		public CharacterSpawner spawner;

		public int RoundNumber => allRounds.Count;
		public Round CurrentRound => allRounds.Last();

		[SerializeField]
		private Round roundPrefab = null;

		[SerializeField]
		private string battleArena = null;

		private void Awake()
		{
			spawner = GetComponent<CharacterSpawner>();
			allRounds = new List<Round>();
		}

		public void Setup()
		{
			SceneManager.LoadScene(battleArena, LoadSceneMode.Additive);
			
			
			matchCreatedEvent.Invoke();
		}

		public void Begin()
		{
			StartNewRound();
			
			
			matchBegunEvent.Invoke();
		}

		public void End()
		{
			Logging.Log("Match ended!", this);
			
			
			matchEndedEvent.Invoke();
		}

		public void Remove()
		{
			// Unload the scene etc..

			gameObject.SetActive(false);
			
			matchRemovedEvent.Invoke();
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
			gameObject.SetActive(false);
		}
	}
}