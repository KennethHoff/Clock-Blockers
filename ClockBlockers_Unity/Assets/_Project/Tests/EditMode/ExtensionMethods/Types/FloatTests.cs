using Between_Names.Utility;

using NUnit.Framework;


namespace ClockBlockers.Tests.EditMode.ExtensionMethods.Types
{
	public class FloatTests
	{
		[Test]
		public void FloatRoundingToCeil()
		{
			const float testFloat = 1235.5f;

			float roundedFloat = testFloat.Round(0);

			Assert.AreEqual(1236f, roundedFloat);
		}

		[Test]
		public void FloatRoundingToFloor()
		{
			const float testFloat = 1235.4f;

			float roundedFloat = testFloat.Round(0);

			Assert.AreEqual(1235f, roundedFloat);
		}

		[Test]
		public void FloatRound3Decimals()
		{
			const float testFloat = 1200.43215f;

			float roundedFloat = testFloat.Round(3);

			Assert.AreEqual(1200.432f, roundedFloat);
		}

		[Test]
		public void FloatArrayScaling()
		{
			float[] testArray = new [] {123f, 45321f, 54321f, -123f, 45321f};

			float[] scaledArray = testArray.Scale(2.5f);
			
			Assert.AreEqual(new [] { 307.5f, 113302.5f, 135802.5f, -307.5f, 113302.5f}, scaledArray);
		} 
	}
}