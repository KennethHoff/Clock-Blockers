using System;


namespace ClockBlockers.DataStructures
{
	[Serializable]
	public class FloatReference : IPropertyReference<float>
	{
		public bool useConstant = true;
		public float constantValue;
		public FloatVariable variable;

		public float Value
		{
			get => useConstant ? constantValue : variable.value;
			set
			{
				if (useConstant)
				{
					constantValue = value;
				}
				else
				{
					variable.value = value;
				}
			}
		}

		public static implicit operator float(FloatReference reference)
		{
			return reference.Value;
		}
	}

	public interface IPropertyReference<T>
	{
		T Value { get; set; }
	}
}