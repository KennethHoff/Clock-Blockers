using Between_Names.Property_References;

using UnityEditor;

using UnityEngine;


namespace BetweenNames.Editor.Property_References
{
	// TODO: Improve the convenience, and usability of the 'Reference' Drawers.
	// If you've input a constant value into the field, and then found out you'd rather have it be a variable field (By selecting the 'Variable' option in the dropdown),
	// grant a popup asking you 'Do you want to convert the current constant into a variable'.
	// Answering no simply does nothing, but answering yes grants you the choice of the folder for where to store it, as well as the name (Defaulted to the name and location of the Field itself)
	
	// Additionally, if you're holding a 'FloatReference', you should be able to drop it onto FloatReference that's set to Constant
	
	[CustomPropertyDrawer(typeof(FloatReference))]
	public class FloatReferenceDrawer : PropertyDrawer
	{
		/// <summary>
        /// Options to display in the popup to select constant or variable.
        /// </summary>
        private readonly string[] popupOptions = 
            { "Use Constant", "Use Variable" };

        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
	        if (popupStyle == null)
	        {
		        popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions")) {imagePosition = ImagePosition.ImageOnly};
	        }

	        label = EditorGUI.BeginProperty(position, label, property);
	        position = EditorGUI.PrefixLabel(position, label);

	        EditorGUI.BeginChangeCheck();

	        // Get properties
	        SerializedProperty useConstant = property.FindPropertyRelative("useConstant");
	        SerializedProperty constantValue = property.FindPropertyRelative("constantValue");
	        SerializedProperty variable = property.FindPropertyRelative("variable");

	        // Calculate rect for configuration button
	        var buttonRect = new Rect(position);
	        buttonRect.yMin += popupStyle.margin.top;
	        buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
	        position.xMin = buttonRect.xMax;

	        // Store old indent level and set it to 0, the PrefixLabel takes care of it
	        int indent = EditorGUI.indentLevel;
	        EditorGUI.indentLevel = 0;

	        int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, popupOptions, popupStyle);

	        useConstant.boolValue = result == 0;

	        EditorGUI.PropertyField(position,
		        useConstant.boolValue ? constantValue : variable,
		        GUIContent.none);

	        if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

	        EditorGUI.indentLevel = indent;
	        EditorGUI.EndProperty();
        }
	}
}