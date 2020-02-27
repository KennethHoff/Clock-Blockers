using ClockBlockers.Characters.Scripts;
using ClockBlockers.DataStructures;
using ClockBlockers.Utility;

using UnityEngine;
using UnityEngine.Events;


namespace ClockBlockers.GameControllers {
	public class Act : MonoBehaviour
	{

		
		[SerializeField]
		private UnityEvent newActEvent;
		
		[SerializeField]
		private FloatReference timeWhenActStarted;

		[SerializeField]
		private FloatReference timeSinceActStarted;

		private void Awake()
		{
			timeWhenActStarted.Value = Time.time;
			
			newActEvent.Invoke();
			
			SpawnAllClones();
		}

		private void SpawnAllClones()
		{
			Logging.instance.Log("Spawned all clones");
		}
	}
}