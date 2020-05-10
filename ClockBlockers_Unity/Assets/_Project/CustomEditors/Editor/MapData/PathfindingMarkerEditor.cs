using System;

using Unity.Burst;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData.Editor
{
	[CustomEditor(typeof(PathfindingMarker))]
	[BurstCompile]
	public class PathfindingMarkerEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			var myScript = (PathfindingMarker) target;
		}
		
		private static void CreateButton(Action action, string buttonText)
		{
			if (GUILayout.Button(buttonText)) action.Invoke();
		}
	}
}