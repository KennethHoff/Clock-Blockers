using System;

using ClockBlockers.DataStructures;
using ClockBlockers.Events;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace ClockBlockers.GameControllers
{
	public class Match : MonoBehaviour
	{

		[SerializeField]
		private UnityEvent newMatchEvent;
		

		public IntReference roundNumber;
		private Round _currentRound;

		[SerializeField]
		private Round roundPrefab;

		
		[SerializeField]
		private string battleArena;
		
		private void Awake()
		{
			StartNewRound();
			newMatchEvent.Invoke();
			
			SceneManager.LoadScene(battleArena, LoadSceneMode.Additive);
		}

		private void StartNewRound()
		{
			_currentRound = Instantiate(roundPrefab);
			roundNumber.Value++;
		}
	}
}