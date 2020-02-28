using System.Globalization;

using UnityEditor;

using UnityEngine;


namespace Between_Names.Property_References.Editor
{
	// TODO: Improve the convenience, and usability of the 'Reference' Drawers.
	// If you've input a constant value into the field, and then found out you'd rather have it be a variable field (By selecting the 'Variable' option in the dropdown),
	// grant a popup asking you 'Do you want to convert the current constant into a variable'.
	// Answering no simply does nothing, but answering yes grants you the choice of the folder for where to store it, as well as the name (Defaulted to the name and location of the Field itself)
	
	// Additionally, if you're holding a 'FloatReference', you should be able to drop it onto FloatReference that's set to Constant
	
	[CustomPropertyDrawer(typeof(FloatReference))]
	public class FloatReferenceDrawer : PropertyDrawer
	{
		private const string BoolString = "useConstant";
		private const string ConstString = "constantValue";
		private const string VariableString = "variable";
		private const int CubeSize = 20;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			bool useConstant = property.FindPropertyRelative(BoolString).boolValue;

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			// Constant/Variable swapping 'button'.
			var rect = new Rect(position.position, Vector2.one * CubeSize);

			// ReSharper disable once StringLiteralTypo
			const string buttonTexture = "animationdopesheetkeyframe";
			
			if (CreateDropdown(rect, buttonTexture))
			{
				CreateToggle(property, useConstant);
			}
			
			position.position += Vector2.right * CubeSize;

			if (useConstant)
			{			
				float value = property.FindPropertyRelative(ConstString).floatValue;
				string newValue = EditorGUI.TextField(position, value.ToString(CultureInfo.CurrentCulture));
				float.TryParse(newValue, out value);
				property.FindPropertyRelative(ConstString).floatValue = value;
			}
			else
			{
				EditorGUI.ObjectField(position, property.FindPropertyRelative(VariableString), GUIContent.none);
			}


			EditorGUI.EndProperty();
		}

		private static bool CreateDropdown(Rect rect, string buttonTexture)
		{
			return EditorGUI.DropdownButton(rect,
				new GUIContent(EditorGUIUtility.IconContent(buttonTexture)),
				FocusType.Keyboard,
				new GUIStyle { fixedWidth = 50f, border = new RectOffset(1, 1, 1, 1) });
		}

		private void CreateToggle(SerializedProperty property, bool useConstant)
		{
			var menu = new GenericMenu();
			menu.AddItem(new GUIContent(ConstString),
				useConstant,
				() => SetProperty(property, true));

			menu.AddItem(new GUIContent(VariableString),
				!useConstant,
				() => SetProperty(property, false));

			menu.ShowAsContext();
		}

		private void SetProperty(SerializedProperty property, bool value)
		{
			SerializedProperty propRelative = property.FindPropertyRelative(BoolString);
			propRelative.boolValue = value;
			property.serializedObject.ApplyModifiedProperties();
		}
	}
}