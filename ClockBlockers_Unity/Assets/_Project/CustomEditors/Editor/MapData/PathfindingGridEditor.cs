using System;

using ClockBlockers.MapData;

using Unity.Burst;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.CustomEditors.Editor.MapData
{
	[CustomEditor(typeof(PathfindingGrid))]
	[BurstCompile]
	public class PathfindingGridEditor : UnityEditor.Editor
	{
		// TODO: Create a custom inspector based on the selected option
		// Only show 'Ray-based Marker Generation' settings if 'Ray' is the selected radio button, and so on..

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		
			var myScript = (PathfindingGrid) target;

			EditorGUILayout.BeginHorizontal("box");
			CreateButton(myScript.GenerateMarkers, "Generate Markers");
			CreateButton(myScript.ClearMarkers, "Clear Markers");
			EditorGUILayout.EndHorizontal();
			
			CreateButton(myScript.RetrieveMarkers, "Retrieve Markers");


			CreateButton(myScript.ResetMarkerGizmos, "Reset Marker Gizmos");
			CreateButton(myScript.GenerateMarkerConnections, "Generate marker adjacencies");

		}
		
		private static void CreateButton(Action action, string buttonText)
		{
			if (GUILayout.Button(buttonText)) action.Invoke();
		}
	}
}