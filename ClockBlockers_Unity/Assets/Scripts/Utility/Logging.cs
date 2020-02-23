using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace ClockBlockers.Utility {
    public static class Logging
    {
        public static void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        public static void LogError(object message, Object context)
        {
            Debug.LogError(message, context);
        }
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
    }
}
