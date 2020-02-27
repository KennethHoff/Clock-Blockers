using System;

using ClockBlockers.DataStructures;

using UnityEngine;
using UnityEngine.Events;


namespace ClockBlockers.GameControllers {
	public class Round : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent newRoundEvent;
		
		private Act _currentAct;
		
		public IntReference actNumber;

		[SerializeField]
		private Act actPrefab;

		private void Awake()
		{
			_currentAct = null;
			actNumber.Value = 0;
			
			newRoundEvent.Invoke();
			
			StartNewAct();
		}

		public void StartNewAct()
		{
			_currentAct = Instantiate(actPrefab);
			actNumber.Value++;
		}
	}
}
