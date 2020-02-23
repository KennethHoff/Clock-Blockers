using ClockBlockers.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace ClockBlockers.GameControllers {
    public class PlayerManagement : MonoBehaviour
    {
        [UsedImplicitly]
        public void OnPlayerJoined()
        {
            Logging.Log("New player joined!");
        }
    }
}
