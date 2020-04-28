using System;

using ClockBlockers.MapData.Grid;

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

			CreateButton(() => myScript.GenerateAllMarkers(), "Generate markers");

			CreateButton(myScript.ClearMarkers, "Remove all markers");
			
			CreateButton(myScript.GenerateMarkerAdjacencies, "Generate marker adjacencies");

			CreateButton(myScript.ResetAllMarkerGizmos, "Reset Marker Colors");

			// CreateButton(myScript.CreateSubGrids, "Create Sub-grids");
		}

		private static void CreateButton(Action action, string buttonText)
		{
			if (GUILayout.Button(buttonText)) action.Invoke();
		}
	}
	
}