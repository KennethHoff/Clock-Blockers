using Between_Names.Utility;

using NUnit.Framework;

using UnityEngine;


namespace ClockBlockers.Tests.EditMode.ExtensionMethods.Types
{
	public class Vector2Tests
	{
		[Test]
		public void Vector3ToFloatArray()
		{
			var testVector = new Vector2(10, 5);

			float[] floatArrayedInput = testVector.ToFloatArray();

			Assert.AreEqual(new float[] {10f, 5f}, floatArrayedInput);
		}
	}
}