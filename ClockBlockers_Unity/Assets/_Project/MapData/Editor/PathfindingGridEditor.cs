using System;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData.Editor
{
	[CustomEditor(typeof(PathfindingGrid))]
	public class PathfindingGridEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var myScript = (PathfindingGrid) target;

			CreateButton(myScript.GenerateAllMarkers, "Generate markers");

			CreateButton(myScript.ClearMarkers, "Remove all markers");
			
			CreateButton(myScript.GenerateMarkerAdjacencies, "Generate marker adjacencies");

			CreateButton(myScript.ResetAllMarkerGizmos, "Reset Marker Colors");
		}

		private static void CreateButton(Action action, string buttonText)
		{
			if (GUILayout.Button(buttonText)) action.Invoke();
		}
	}
	
}