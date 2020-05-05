using UnityEditor;


namespace ClockBlockers.MapData.MarkerGenerators.Editor
{
	[CustomEditor(typeof(OrbitalRayMarkerGenerator))]
	class OrbitalRayMarkerGeneratorEditor : AutomatedMarkerGeneratorEditor
	{
		public override void OnInspectorGUI()
		{
			myScript = (OrbitalRayMarkerGenerator) target;
			base.OnInspectorGUI();
			
		}
	}
}