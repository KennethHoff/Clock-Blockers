using System;

using UnityEngine;


// ReSharper disable once CheckNamespace
namespace Between_Names.Utility
{
	public static class ExtensionMethods {
		
		public static float[] ToFloatArray(this Vector3 vector3)
		{
			return new[]
			{
				vector3.x,
				vector3.y,
				vector3.z
			};
		}

		/// <param name="f"></param>
		/// <param name="decimalPlaces">Should not be higher than 4, otherwise I can't guarantee precision</param>
		public static float Round(this float f, int decimalPlaces)
		{
			var multiplier = (int) Mathf.Pow(10, decimalPlaces);

			return Mathf.Round(f * multiplier) / multiplier;	
		}

		public static float[] ToFloatArray(this Vector2 vector2)
		{
			return new[]
			{
				vector2.x,
				vector2.y
			};
		}

		public static float[] Scale(this float[] array, float scalar = 1)
		{
			var result = new float[array.Length];

			for (var i = 0;
				i < array.Length;
				i++)
			{
				result[i] = array[i] * scalar;
			}

			return result;
		}

		public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
		{
			var multiplier = (int) Mathf.Pow(10, decimalPlaces);
			
			return new Vector3(
				Mathf.Round(vector3.x * multiplier) / multiplier,
				Mathf.Round(vector3.y * multiplier) / multiplier,
				Mathf.Round(vector3.z * multiplier) / multiplier);
		}
	}
}