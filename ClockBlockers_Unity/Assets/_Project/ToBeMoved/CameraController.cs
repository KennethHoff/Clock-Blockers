using Between_Names.Property_References;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.ToBeMoved {
	[BurstCompile]
	public class CameraController : MonoBehaviour
	{
		[SerializeField]
		private FloatReference minCamAngle = null;
		
		[SerializeField]
		private FloatReference maxCamAngle = null;
		
		public void Rotate(float input)
		{
			Vector3 currentAngle = transform.rotation.eulerAngles;

			float newX = currentAngle.x - input;
			
			float antiAliasedRotation = newX > 180 ? newX - 360 : newX;
			
			float finalRotation =  Mathf.Clamp(antiAliasedRotation, minCamAngle, maxCamAngle);

			var rotationVector = new Vector3(finalRotation, 0, 0);

			transform.localRotation = Quaternion.Euler(rotationVector);
		}
	}
}