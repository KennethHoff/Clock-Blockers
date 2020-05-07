using System;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData.Editor
{
	[CustomEditor(typeof(PathfindingGrid))]
	public class PathfindingGridEditor : UnityEditor.Editor
	{
		// TODO: Create a custom inspector based on the selected option
		// Only show 'Ray-based Marker Generation' settings if 'Ray' is the selected radio button, and so on..

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		
			var myScript = (PathfindingGrid) target;
			
			CreateButton(myScript.ResetMarkerGizmos, "Reset Marker Gizmos");
		}
		
		private static void CreateButton(Action action, string buttonText)
		{
			if (GUILayout.Button(buttonText)) action.Invoke();
		}
	}
}