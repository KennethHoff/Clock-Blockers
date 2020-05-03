using UnityEditor;


namespace ClockBlockers.MapData.Editor
{
	[CustomEditor(typeof(SurfaceColliderMarkerGenerator))]
	class SurfaceColliderMarkerGeneratorEditor : MarkerGeneratorBaseEditor
	{
		public override void OnInspectorGUI()
		{
			myScript = (SurfaceColliderMarkerGenerator) target;	
			base.OnInspectorGUI();
		}
	}
}