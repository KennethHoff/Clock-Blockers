using UnityEditor;


namespace ClockBlockers.MapData.Editor
{
	[CustomEditor(typeof(OrbitalRayMarkerGenerator))]
	class OrbitalRayMarkerGeneratorEditor : MarkerGeneratorBaseEditor
	{
		public override void OnInspectorGUI()
		{
			myScript = (OrbitalRayMarkerGenerator) target;
			base.OnInspectorGUI();
		}
	}
}