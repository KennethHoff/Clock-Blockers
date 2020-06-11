using Between_Names.Utility;

using NUnit.Framework;

using UnityEngine;
// ReSharper disable InconsistentNaming


namespace ClockBlockers.Tests.EditMode.ExtensionMethods.Types
{
	[TestFixture]
	public class Converting_a_Vector2_into_a_Float_Array
	{
		[Test]
		public void Have_two_elements_whose_values_are_x_and_y()
		{
			var testVector = new Vector2(10, 5);

			float[] floatArrayedInput = testVector.ToFloatArray();

			Assert.IsTrue(floatArrayedInput.Length == 2);

			Assert.AreEqual(new[] {10f, 5f}, floatArrayedInput);
		}
	}
}