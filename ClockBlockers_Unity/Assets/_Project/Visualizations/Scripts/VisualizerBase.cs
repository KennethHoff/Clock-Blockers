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

		public void Create(Vector3 position, Quaternion rotation, Transform parent)
		{
			Instantiate(this, position, rotation, parent);
		}

		public void Create(Vector3 position, Quaternion rotation)
		{
			Instantiate(this, position, rotation);

		}
	}
}