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

		public void Log(object message, Object context)
		{
			Debug.Log(message, context);
			if (_logToFile)
			{
				SaveToFile(message, context);
			}
		}

		private void SaveToFile(object message, Object context = null)
		{
			string jsonData = JsonUtility.ToJson(message);
			
			var writer = new StreamWriter(_jsonSavePath, true);

			using (writer)
			{
				writer.WriteLine(jsonData);
			}
		}

		public void Log(object message)
		{
			Debug.Log(message);
			if (_logToFile)
			{
				SaveToFile(message);
			}
		}

		public void LogWarning(object message, Object context)
		{
			Debug.LogWarning(message, context);
			
			if (_logToFile)
			{
				SaveToFile(message);
			}
		}

		public void LogWarning(object message)
		{
			Debug.LogWarning(message);
			if (_logToFile)
			{
				SaveToFile(message);
			}
		}

		public void LogError(object message, Object context)
		{
			Debug.LogError(message, context);
			if (_logToFile)
			{
				SaveToFile(message, context);
			}
		}

		public void LogError(object message)
		{
			Debug.LogError(message);
			if (_logToFile)
			{
				SaveToFile(message);
			}
		}

		public void LogIncorrectInstantiation(string typeStr, Object context)
		{
			string str = context.name + " was created incorrectly. Missing " + typeStr + ".";
			LogWarning(str, context);
			if (_logToFile)
			{
				SaveToFile(str, context);
			}
		}
	}
}