using ClockBlockers.Utility;

using UnityEngine;



namespace ClockBlockers.GameControllers
{
	public class PlayerManagement : MonoBehaviour
	{
		public void OnPlayerJoined()
		{
			Logging.Log("New player joined!");
		}
	}
}