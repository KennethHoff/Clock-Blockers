using System;


namespace Between_Names.Property_References
{
	[Serializable]
	public class IntReference : IPropertyReference<int>
	{
		public bool useConstant = true;
		public int constantValue;
		public IntVariable variable;

		public int Value
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
		public static implicit operator int(IntReference reference)
		{
			return reference.Value;
		}
	}
}