using Between_Names.Utility;

using UnityEngine;

using NUnit.Framework;
// ReSharper disable InconsistentNaming


namespace ClockBlockers.Tests.EditMode.ExtensionMethods.Types
{
	[TestFixture]
	public class Converting_a_Vector3_into_a_Float_Array
	{
		[Test]
		public void Have_three_elements_whose_values_are_x_y_and_z()
		{
			var testVector = new Vector3(10, 5, 10);

			float[] floatArrayedInput = testVector.ToFloatArray();
			
			Assert.IsTrue(floatArrayedInput.Length == 3);

			Assert.AreEqual(new[] {10f, 5f, 10f}, floatArrayedInput);
		}
	}
	[TestFixture]
	public class Rounding_a_Vector3 {

		[Test]
		public void All_elements_rounded_independently()
		{
			var testVector = new Vector3(10.524f, 532.23f, 1234);

			Vector3 roundedVector = testVector.Round(0);

			Assert.AreEqual(new Vector3(11, 532, 1234), roundedVector);
		}
	}
}