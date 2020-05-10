using System.Collections;

using Between_Names.Property_References;

using UnityEngine;


namespace ClockBlockers.Visualizations
{
	internal class QuestMarker3D : VisualizerBase
	{
		[SerializeField]
		private AnimationCurve bobbingCurve = null;

		[SerializeField]
		private FloatReference bobbingMultiplier = null;

		private void Start()
		{
			StartCoroutine(BobbingRoutine());
		}

		private IEnumerator BobbingRoutine()
		{
			float elapsedTime = 0;

			// float curveLength = bobbingCurve.length;

			// float startY = thisTransform.position.y;

			Vector3 startPos = thisTransform.position;

			while (true)
			{
				// elapsedTime %= bobbingCurve.length;
				float currentYPos = bobbingCurve.Evaluate(elapsedTime) * bobbingMultiplier;

				Vector3 newPos = startPos;
				
				newPos.y += currentYPos;
				
				thisTransform.position = newPos;

				elapsedTime += Time.deltaTime;
				
				yield return null;
			}
			
		}

	}
}