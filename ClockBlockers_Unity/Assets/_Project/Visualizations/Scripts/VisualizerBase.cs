using Between_Names.Property_References;

using UnityEngine;


namespace ClockBlockers.Visualizations
{
	public abstract class VisualizerBase : MonoBehaviour
	{
		[SerializeField]
		private FloatReference lifetime = null;

		protected Transform thisTransform;

		protected virtual void Awake()
		{
			thisTransform = GetComponent<Transform>();
			Destroy(gameObject, lifetime);
		}
	}
}