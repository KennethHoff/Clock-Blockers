using UnityEngine;


namespace Between_Names.Property_References
{
	// TODO: Find out how to make this more generic

	[CreateAssetMenu(fileName = "Int Variable", menuName = "Field Variables/Int Variable")]
	public class IntVariable : ScriptableObject
	{
		public int value;
	}
}