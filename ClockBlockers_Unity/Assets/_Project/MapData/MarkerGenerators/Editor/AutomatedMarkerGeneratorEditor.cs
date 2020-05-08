using System;

using ClockBlockers.Utility;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData.MarkerGenerators.Editor
{
	[CustomEditor(typeof(AutomatedMarkerGenerator))]
	public class AutomatedMarkerGeneratorEditor : UnityEditor.Editor
	{
		protected AutomatedMarkerGenerator myScript = null;
		
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (myScript == null)
			{
				Logging.Log("Please create a custom Editor for " + GetType() + ".");
				return;
			}
			
			// GUILayout.BeginHorizontal("box");

			CreateButton(myScript.GenerateAllMarkers, "Generate markers");
			// CreateButton(myScript.MergeMarkers, "Merge Markers");
			
			// GUILayout.EndHorizontal();
			
			CreateButton(myScript.ClearMarkers, "Remove all markers");
			
			CreateButton(myScript.GenerateMarkerConnections, "Generate marker adjacencies");

			
		}

		protected static void CreateButton(Action action, string buttonText)
		{
			if (GUILayout.Button(buttonText)) action.Invoke();
		}
	}
}