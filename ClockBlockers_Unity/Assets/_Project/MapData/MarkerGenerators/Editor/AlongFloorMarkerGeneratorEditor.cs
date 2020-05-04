using UnityEditor;


namespace ClockBlockers.MapData.MarkerGenerators.Editor
{
	[CustomEditor(typeof(AlongFloorMarkerGenerator))]
	internal class AlongFloorMarkerGeneratorEditor : AutomatedMarkerGeneratorEditor
	{
		public override void OnInspectorGUI()
		{
			myScript = (AlongFloorMarkerGenerator) target;
			base.OnInspectorGUI();
		}
	}
}