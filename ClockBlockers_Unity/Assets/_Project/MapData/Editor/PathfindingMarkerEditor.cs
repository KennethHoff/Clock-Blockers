using System;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData.Editor
{
	[CustomEditor(typeof(PathfindingMarker))]
	public class PathfindingMarkerEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			var myScript = (PathfindingMarker) target;

			CreateButton(myScript.GetOrAddToGridDictionary, "Get Or Add Marker To Dictionary");
		}
		
		private static void CreateButton(Action action, string buttonText)
		{
			if (GUILayout.Button(buttonText)) action.Invoke();
		}
	}
}