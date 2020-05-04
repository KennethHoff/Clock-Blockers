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

			// myScript = (MarkerGeneratorBase) target;	

			CreateButton(() => myScript.GenerateAllMarkers(), "Generate markers");

			CreateButton(myScript.ClearMarkers, "Remove all markers");
			
			CreateButton(myScript.GenerateMarkerAdjacencies, "Generate marker adjacencies");
		}

		protected static void CreateButton(Action action, string buttonText)
		{
			if (GUILayout.Button(buttonText)) action.Invoke();
		}
	}
}