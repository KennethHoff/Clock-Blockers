using System.Collections.Generic;

using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using ClockBlockers.ReplaySystem;

using Unity.Burst;


// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global


namespace ClockBlockers.Utility
{
	[BurstCompile]
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


		public static int GetLayerInt(this LayerMask aMask)
		{
			var val = (uint)aMask.value;
			if (val  == 0) return -1;
			for (var i = 0; i < 32; i++)
			{
				if( (val & (1<<i)) != 0)
				{
					return i;
				}
			}
			return -1;
		}
	}
}