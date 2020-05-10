﻿using System;


namespace Between_Names.Property_References
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
		
		public string ToString(string format)
		{
			// ReSharper disable once SpecifyACultureInStringConversionExplicitly
			return Value.ToString(format);
		}

		public override string ToString()
		{
			// ReSharper disable once SpecifyACultureInStringConversionExplicitly
			return Value.ToString();
		}
	}
}