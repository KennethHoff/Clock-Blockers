using ClockBlockers.BetweenNames.Attributes;

using UnityEditor;

using UnityEngine;


// http://answers.unity.com/answers/801283/view.html
namespace ClockBlockers.BetweenNames.Editor.Attributes
{
 
	[CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
	public class ReadOnlyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property,
			GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
 
		public override void OnGUI(Rect position,
			SerializedProperty property,
			GUIContent label)
		{
			GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = true;
		}
	}
}