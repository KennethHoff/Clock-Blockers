using UnityEngine;


namespace ClockBlockers.Targetting
{
	public class CameraForwardRayProvider : MonoBehaviour, IRayProvider
	{
		public Ray CreateRay(Camera cam, Vector2 mousePos)
		{
			Transform camTransform = cam.transform;
			return new Ray(camTransform.position, camTransform.forward);
		}
	}
}