using Unity.Burst;

using UnityEngine;
using UnityEngine.Events;


namespace ClockBlockers.Events {
	[BurstCompile]
	public class GameEventListener : MonoBehaviour
	{
		public GameEvent @event;
		public UnityEvent response;

		private void OnEnable()
		{
			@event.RegisterListener(this);
		}

		private void OnDisable()
		{
			@event.UnregisterListener(this);
		}

		public void OnEventRaised()
		{
			response.Invoke();
		}
	}
}