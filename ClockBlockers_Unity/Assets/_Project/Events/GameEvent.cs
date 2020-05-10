using System.Collections.Generic;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.Events {
	[CreateAssetMenu]
	[BurstCompile]
	public class GameEvent : ScriptableObject
	{
		private readonly List<GameEventListener> _listeners = new List<GameEventListener>();

		public void Raise()
		{
			if (_listeners.Count == 0) return;
			
			for (int i = _listeners.Count-1; i >= 0; i--)
			{
				_listeners[i].OnEventRaised();
			}
		}

		public void RegisterListener(GameEventListener listener)
		{
			_listeners.Add(listener);
		}

		public void UnregisterListener(GameEventListener listener)
		{
			_listeners.Remove(listener);
		}
	}
}