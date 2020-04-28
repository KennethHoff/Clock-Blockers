using UnityEngine;
// ReSharper disable UnusedMember.Global


namespace ClockBlockers.Utility
{
	internal static class ExtensionMethods {
		public static float[] ToFloatArray(this Vector3 vector3)
		{
			return new[]
			{
				vector3.x,
				vector3.y,
				vector3.z
			};
		}

		public static float Clamp(this float f, float min, float max)
		{
			if (f <= min) f = min;
			if (f >= max) f = max;
			return f;
		}

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
			// ReSharper disable once SuggestVarOrType_Elsewhere
			float[] result = new float[array.Length];

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
			float multiplier = 1;
			for (var i = 0; i < decimalPlaces; i++)
			{
				multiplier *= 10f;
			}

			return new Vector3(
				Mathf.Round(vector3.x * multiplier) / multiplier,
				Mathf.Round(vector3.y * multiplier) / multiplier,
				Mathf.Round(vector3.z * multiplier) / multiplier);
		}
	}
}