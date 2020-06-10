using Between_Names.Utility;

using UnityEngine;

using NUnit.Framework;


namespace ClockBlockers.Tests.EditMode.ExtensionMethods.Types
{
	public class Vector3Tests
	{
		[Test]
		public void Vector3ToFloatArray()
		{
			var testVector = new Vector3(10, 5, 10);

			float[] floatArrayedInput = testVector.ToFloatArray();

			Assert.AreEqual(new float[] {10f, 5f, 10f}, floatArrayedInput);
		}

		[Test]
		public void Vector3RoundToCeil()
		{
			var testVector = new Vector3(10.5231f, 123.523f, 231.54f);

			Vector3 roundedVector = testVector.Round(0);

			Assert.AreEqual(new Vector3(11, 124, 232), roundedVector);
		}
		
		
		[Test]
		public void Vector3RoundToFloor()
		{
			var testVector = new Vector3(10.4231f, 123.423f, 231.44f);

			Vector3 roundedVector = testVector.Round(0);

			Assert.AreEqual(new Vector3(10, 123, 231), roundedVector);
		}

		[Test]
		public void Vector3RoundMixed()
		{
			var testVector = new Vector3(10.524f, 532.23f, 1234);

			Vector3 roundedVector = testVector.Round(0);

			Assert.AreEqual(new Vector3(11, 532, 1234), roundedVector);
		}
		
	}
}