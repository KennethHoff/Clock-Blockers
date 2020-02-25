using System.Collections.Generic;

using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using ClockBlockers.NewReplaySystem;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global


namespace ClockBlockers.Utility
{
	public static class DataManipulation
	{
		private static readonly BinaryFormatter Formatter = new BinaryFormatter();
		private static readonly string ActionSaveFile = Application.persistentDataPath + "/save.CBAction";


		public static void SaveActions(CharacterAction[] characterActions)
		{
			FileStream stream = File.Exists(ActionSaveFile)
				? File.OpenWrite(ActionSaveFile)
				: File.Create(ActionSaveFile);

			Formatter.Serialize(stream, characterActions);
			stream.Close();
		}

		public static void SaveActions(LinkedList<CharacterAction> characterActions)
		{
			FileStream stream = File.Exists(ActionSaveFile)
				? File.OpenWrite(ActionSaveFile)
				: File.Create(ActionSaveFile);

			Formatter.Serialize(stream, characterActions);
			stream.Close();
		}


		public static CharacterAction[] LoadActions()
		{
			FileStream stream;

			if (File.Exists(ActionSaveFile))
			{
				stream = File.OpenRead(ActionSaveFile);
			}
			else
			{
				Logging.LogWarning("No save file!");
				return null;
			}

			var actions = (CharacterAction[]) Formatter.Deserialize(stream);
			stream.Close();
			return actions;
		}
	}
}