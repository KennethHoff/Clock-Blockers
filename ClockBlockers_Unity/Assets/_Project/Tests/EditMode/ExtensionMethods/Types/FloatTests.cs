using Between_Names.Utility;

using NUnit.Framework;

using UnityEngine;


// ReSharper disable InconsistentNaming


namespace ClockBlockers.Tests.EditMode.ExtensionMethods.Types
{
	[TestFixture]
	public class Rounding_a_Float
	{
		[TestFixture]
		public class Requested_decimals_is_0
		{
			[Test]
			public void Round_up_and_remove_all_decimals_if_first_decimal_5_or_higher()
			{
				const float testFloat = 1235.5f;

				float roundedFloat = testFloat.Round(0);

				Assert.AreEqual(1236f, roundedFloat);
			}

			[Test]
			public void Round_down_and_remove_all_decimals_if_first_decimal_4_or_lower()
			{
				const float testFloat = 1235.4f;

				float roundedFloat = testFloat.Round(0);

				Assert.AreEqual(1235f, roundedFloat);
			}

			[Test]
			public void Do_nothing_if_there_is_no_decimal_expansion()
			{
				const float testFloat = 1235f;

				float roundedFloat = testFloat.Round(0);

				Assert.AreEqual(testFloat, roundedFloat);
			}
		}

		// Doesn't really work :/
		// [TestFixture]
		// public class Requested_decimals_n_is_more_than_0_but_4_or_less
		// {
		// 	[Test]
		// 	public void Round_up_nth_decimal_if_subsequent_decimal_is_5_or_higher()
		// 	{
		// 		const float testFloat = 1200.189465f;
		//
		// 		float roundedFloat = testFloat.Round(4);
		//
		// 		const float expected = 1200.1894f;
		//
		// 		float absDifferenceBetweenExpectedAndResult = Mathf.Abs(expected-roundedFloat);
		// 		
		// 		Assert.Zero(absDifferenceBetweenExpectedAndResult);
		// 	}
		//
		// 	[Test]
		// 	public void Round_down_nth_decimal_if_subsequent_decimal_is_4_or_lower()
		// 	{
		// 		const float testFloat = 1200.189449f;
		//
		// 		float roundedFloat = testFloat.Round(4);
		//
		// 		const float expected = 1200.1894f;
		//
		// 		float absDifferenceBetweenExpectedAndResult = Mathf.Abs(expected-roundedFloat);
		// 		
		// 		Assert.Zero(absDifferenceBetweenExpectedAndResult);
		// 	}
		// }
	}

	[TestFixture]
	public class Scaling_an_Array_of_Floats
	{
		[Test]
		public void all_elements_scaled_same_amount()
		{
			float[] testArray = {123f, 45321f, 54321f, -123f, 45321f};

			float[] scaledArray = testArray.Scale(2.5f);
			
			Assert.AreEqual(new [] { 307.5f, 113302.5f, 135802.5f, -307.5f, 113302.5f}, scaledArray);
		} 
	}
}