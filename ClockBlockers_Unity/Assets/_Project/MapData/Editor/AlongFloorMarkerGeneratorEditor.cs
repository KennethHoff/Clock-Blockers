using UnityEditor;


namespace ClockBlockers.MapData.Editor
{
	[CustomEditor(typeof(AlongFloorMarkerGenerator))]
	class AlongFloorMarkerGeneratorEditor : MarkerGeneratorBaseEditor
	{
		public override void OnInspectorGUI()
		{
			myScript = (AlongFloorMarkerGenerator) target;
			base.OnInspectorGUI();
		}
	}
}