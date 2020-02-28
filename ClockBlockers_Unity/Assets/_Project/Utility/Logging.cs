using System.IO;

using UnityEngine;

using Object = UnityEngine.Object;
// ReSharper disable UnusedMember.Global


// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace ClockBlockers.Utility
{
	public class Logging
	{
		public static Logging instance;
		private string _jsonSavePath;
		

		private bool _logToFile;

		public Logging(string jsonFileName, bool logToFile)
		{
			_logToFile = logToFile;
			_jsonSavePath = Application.persistentDataPath + "/" + jsonFileName + ".json";
			instance = this;

			File.Delete(_jsonSavePath);
		}

		public static void Log(object message, Object context)
		{
			Debug.Log(message, context);
			
			instance.SaveToFile(message, context);
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
			
			instance.SaveToFile(message);
		}

		public static void LogWarning(object message, Object context)
		{
			Debug.LogWarning(message, context);
			
			instance.SaveToFile(message, context);
		}

		public static void LogWarning(object message)
		{
			Debug.LogWarning(message);
			instance.SaveToFile(message);
		}

		public static void LogError(object message, Object context)
		{
			Debug.LogError(message, context);
			instance.SaveToFile(message, context);
		}

		public static void LogError(object message)
		{
			Debug.LogError(message);
			instance.SaveToFile(message);
		}

		public static void LogIncorrectInstantiation(string typeStr, Object context)
		{
			string str = context.name + " was created incorrectly. Missing " + typeStr + ".";
			LogWarning(str, context);
		}
	}
}