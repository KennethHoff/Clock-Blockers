using System;
using System.IO;

using ClockBlockers.ReplaySystem.ReplayRunner;

using Unity.Burst;

using UnityEngine;

using Object = UnityEngine.Object;

namespace ClockBlockers.Utility
{
	[ExecuteAlways][BurstCompile]
	public class Logging : MonoBehaviour
	{

		// TODO: Add Output-To-File support
		public static void Log(object message, Object context)
		{
			Debug.Log(message, context);
		}

		public static void Log(object message)
		{
			Debug.Log(message);
		}

		public static void LogWarning(object message, Object context)
		{
			Debug.LogWarning(message, context);
		}

		public static void LogWarning(object message)
		{
			Debug.LogWarning(message);
		}

		public static void LogError(object message, Object context)
		{
			Debug.LogError(message, context);
		}

		public static void LogError(object message)
		{
			Debug.LogError(message);
		}

		private static void LogIncorrectInstantiation(Object context, string typeStr)
		{
			string str = context.name + " was created incorrectly. Missing " + typeStr + ".";
			LogWarning(str, context);
		}

		/// <summary>
		/// Returns true if MonoBehaviour exists. Otherwise returns false.
		/// </summary>
		public static bool CheckIfCorrectMonoBehaviourInstantiation<T>(ref T component, Object context, string typeStr) where T : MonoBehaviour
		{
			if (component != null) return true;

			LogIncorrectInstantiation(context, typeStr);
			return false;
		}


		/// <summary>
		/// Returns true if Component exists. Otherwise returns false.
		/// </summary>
		public static bool CheckIfCorrectComponentInstantiation<T>(ref T component, Object context, string typeStr) where T : Component
		{
			if (component != null) return true;

			LogIncorrectInstantiation(context, typeStr);
			return false;
		}
	}
}