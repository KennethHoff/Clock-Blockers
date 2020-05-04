using UnityEditor;


namespace ClockBlockers.MapData.MarkerGenerators.Editor
{
	[CustomEditor(typeof(SurfaceColliderMarkerGenerator))]
	class SurfaceColliderMarkerGeneratorEditor : AutomatedMarkerGeneratorEditor
	{
		public override void OnInspectorGUI()
		{
			myScript = (SurfaceColliderMarkerGenerator) target;	
			base.OnInspectorGUI();
		}
	}
}