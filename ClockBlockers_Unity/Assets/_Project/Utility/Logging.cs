using System;
using System.IO;

using ClockBlockers.ReplaySystem.ReplayRunner;

using UnityEngine;

using Object = UnityEngine.Object;

namespace ClockBlockers.Utility
{
	public class Logging
	{
		private static Logging _instance;
		private readonly string _jsonSavePath;
		

		private bool _logToFile;

		public Logging(string jsonFileName, bool logToFile)
		{
			_logToFile = logToFile;
			_jsonSavePath = Application.persistentDataPath + "/" + jsonFileName + ".json";
			_instance = this;

			File.Delete(_jsonSavePath);
		}

		public static void Log(object message, Object context)
		{
			Debug.Log(message, context);
			
			_instance.SaveToFile(message, context);
		}

		private void SaveToFile(object message, Object context = null)
		{
			if(!_logToFile) return;
			string jsonData = JsonUtility.ToJson(message);
			
			var writer = new StreamWriter(_jsonSavePath, true);

			using (writer)
			{
				writer.WriteLine(jsonData);
			}
		}

		public static void Log(object message)
		{
			Debug.Log(message);
			
			_instance.SaveToFile(message);
		}

		public static void LogWarning(object message, Object context)
		{
			Debug.LogWarning(message, context);
			
			_instance.SaveToFile(message, context);
		}

		public static void LogWarning(object message)
		{
			Debug.LogWarning(message);
			_instance.SaveToFile(message);
		}

		public static void LogError(object message, Object context)
		{
			Debug.LogError(message, context);
			_instance.SaveToFile(message, context);
		}

		public static void LogError(object message)
		{
			Debug.LogError(message);
			_instance.SaveToFile(message);
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