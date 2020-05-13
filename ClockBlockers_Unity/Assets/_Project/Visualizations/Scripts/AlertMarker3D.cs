using System.Collections;

using UnityEngine;


namespace ClockBlockers.Visualizations
{
	internal class AlertMarker3D : VisualizerBase
	{
		[SerializeField]
		private AnimationCurve alertScaleCurve = null;

		[SerializeField]
		private float alertScaleMultiplier = 1;
		
		private void Start()
		{
			StartCoroutine(AlertScaleRoutine());
		}

		private IEnumerator AlertScaleRoutine()
		{
			float elapsedTime = 0;

			Vector3 startScale = thisTransform.localScale;

			while (true)
			{
				elapsedTime %= alertScaleCurve.length;
				
				float currentScaleMultiplier = alertScaleCurve.Evaluate(elapsedTime) * alertScaleMultiplier;

				thisTransform.localScale = startScale * currentScaleMultiplier;

				elapsedTime += Time.deltaTime;
				
				yield return null;
			}
		}
	}
}