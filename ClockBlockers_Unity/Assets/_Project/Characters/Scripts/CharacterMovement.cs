using Between_Names.Property_References;

using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;

namespace ClockBlockers.Characters
{
	[BurstCompile]
	public class CharacterMovement : MonoBehaviour
	{
		
		[SerializeField]
		private FloatReference moveSpd = null;
		
		[SerializeField]
		private FloatReference jumpHeight = null;

		[SerializeField] 
		private Vector3 velocity;

		[SerializeField]
		private float divFactMulti = 1.25f;

		public bool isGrounded;

		public Vector3 charDimension = new Vector3(0, 2, 0);
		public Vector3 Velocity
		{
			get => velocity;
			set => velocity = value;
		}
		
		public float MoveSpd => moveSpd;

		public float JumpHeight => jumpHeight;


		private void Update()
		{
			velocity /= (1 + Time.deltaTime) * divFactMulti;
			Move(velocity);
		}

		private void LateUpdate()
		{
			CheckCollision();
		}

		private void CheckCollision()
		{
			
			Transform charTransform = transform;
			Vector3 currPos = charTransform.position;
			
			CheckIfGrounded(ref charTransform, ref currPos);
			CheckIfHittingWall(ref charTransform, ref currPos);
			CheckIfHitRoof(ref  charTransform, ref currPos);

		}

		private void CheckIfHittingWall(ref Transform charTransform, ref Vector3 currPos)
		{
			CheckIfHittingLeftWall(ref charTransform, ref currPos);
			CheckIfHittingRightWall(ref charTransform, ref currPos);
			
			
			CheckIfHittingForwardWall(ref charTransform, ref currPos);
			CheckIfHittingBackWall(ref charTransform, ref currPos);

		}

		private bool CheckIfHittingLeftWall(ref Transform charTransform, ref Vector3 currPos)
		{
			float distance = -velocity.x + (charDimension.x / 2);
			RaycastHit[] rays = RayCaster.CastRayAll(new Ray(currPos, Vector3.left), distance);
			if (rays.Length == 0)
			{
				return false;
			}
			
			float largestXPos = float.NaN;

			foreach (RaycastHit ray in rays)
			{
				float rayHitXPos = ray.point.x;
				if (float.IsNaN(largestXPos) || rayHitXPos > largestXPos) largestXPos = rayHitXPos;
				velocity.x = 0;
			}
			
			float floorX = largestXPos + (charDimension.x / 2);
			currPos = new Vector3(floorX, currPos.y, currPos.z);
			charTransform.position = currPos;

			return true;
		}

		private bool CheckIfHittingRightWall(ref Transform charTransform, ref Vector3 currPos)
		{
			float distance = velocity.x + (charDimension.x / 2);
			RaycastHit[] rays = RayCaster.CastRayAll(new Ray(currPos, Vector3.right), distance);
			if (rays.Length == 0)
			{
				return false;
			}
			
			float smallestXPos = float.NaN;

			foreach (RaycastHit ray in rays)
			{
				float rayHitXPos = ray.point.x;
				if (float.IsNaN(smallestXPos) || rayHitXPos < smallestXPos) smallestXPos = rayHitXPos;
				velocity.x = 0;
			}
			
			float floorX = smallestXPos - (charDimension.x / 2);
			currPos = new Vector3(floorX, currPos.y, currPos.z);
			charTransform.position = currPos;

			return true;
		}

		private bool CheckIfHittingForwardWall(ref Transform charTransform, ref Vector3 currPos)
		{
			float distance = velocity.z + (charDimension.z / 2);
			RaycastHit[] rays = RayCaster.CastRayAll(new Ray(currPos, Vector3.forward), distance);
			if (rays.Length == 0)
			{
				return false;
			}
			
			float smallestZPos = float.NaN;

			foreach (RaycastHit ray in rays)
			{
				float rayHitZPos = ray.point.z;
				if (float.IsNaN(smallestZPos) || rayHitZPos < smallestZPos) smallestZPos = rayHitZPos;
				velocity.z = 0;
			}
			
			float floorZ = smallestZPos - (charDimension.z / 2);
			currPos = new Vector3(currPos.x, currPos.y, floorZ);
			charTransform.position = currPos;

			return true;
		}

		private bool CheckIfHittingBackWall(ref Transform charTransform, ref Vector3 currPos)
		{
			float distance = -velocity.z + (charDimension.z / 2);
			RaycastHit[] rays = RayCaster.CastRayAll(new Ray(currPos, Vector3.back), distance);
			if (rays.Length == 0)
			{
				return false;
			}
			
			float largestZPos = float.NaN;

			foreach (RaycastHit ray in rays)
			{
				float rayHitZPos = ray.point.z;
				if (float.IsNaN(largestZPos) || rayHitZPos > largestZPos) largestZPos = rayHitZPos;
				velocity.z = 0;
			}
			
			float floorZ = largestZPos + (charDimension.z / 2);
			currPos = new Vector3(currPos.x, currPos.y, floorZ);
			charTransform.position = currPos;

			return true;
		}

		private void CheckIfGrounded(ref Transform charTransform, ref Vector3 currPos)
		{
			float distance = -velocity.y + (charDimension.y / 2);
			RaycastHit[] rays = RayCaster.CastRayAll(new Ray(currPos, Vector3.down), distance);
			if (rays.Length == 0)
			{
				isGrounded = false;
				return;
			}
			
			float highestYPos = float.NaN;

			foreach (RaycastHit ray in rays)
			{
				float rayHitYPos = ray.point.y;
				if (float.IsNaN(highestYPos) || rayHitYPos > highestYPos) highestYPos = rayHitYPos;
				velocity.y = 0;
			}
			
			float floorY = highestYPos + (charDimension.y / 2);
			currPos = new Vector3(currPos.x, floorY, currPos.z);
			charTransform.position = currPos;
			isGrounded = true;
		}

		private bool CheckIfHitRoof(ref Transform charTransform, ref Vector3 currPos)
		{
			float distance = velocity.y + (charDimension.y / 2);
			RaycastHit[] rays = RayCaster.CastRayAll(new Ray(currPos, Vector3.up), distance);
			if (rays.Length == 0)
			{
				return false;
			}
			
			float smallestYPos = float.NaN;

			foreach (RaycastHit ray in rays)
			{
				float rayHitXPos = ray.point.y;
				if (float.IsNaN(smallestYPos) || rayHitXPos < smallestYPos) smallestYPos = rayHitXPos;
				velocity.y = 0;
			}

			// If the roof is lower than the height of the character, do nothing -- This is a fix to the problem where the player would teleport through the floor if the ceiling is too low.
			// BUG: If you jump while being in this "Roof is too low" state, you'll not be stopped by the roof - you'll go right through as if it didn't exist.
			if (smallestYPos <= charDimension.y + currPos.y) return false;
			
			// Lowest (Y-pos) thing you hit minus the height of the character
			// Meaning, the "feet" is <Height of character> 'units' away from the ceiling
			
			float floorY = smallestYPos - charDimension.y;
			currPos = new Vector3(currPos.x, floorY, currPos.z);
			charTransform.position = currPos;

			return true;
		}

		private void OnDisable()
		{
			Logging.Log("Character Movement Disabled on " + name);
		}


		private Vector3 GetForwardVector(out Vector3 right)
		{
			Transform transformRef = transform;
			Vector3 forward = transformRef.forward;
			right = transformRef.right;
			forward.y = 0;
			right.y = 0;
			forward.Normalize();
			right.Normalize();
			return forward;
		}
		
		
		public void AddVelocity(Vector3 moveVector)
		{
			Velocity += moveVector * moveSpd;
		}

		public void AddVelocityFromInputVector(Vector2 inputVector)
		{
			var moveVector = new Vector3(inputVector.x, 0, inputVector.y);
			AddVelocity(moveVector * Time.deltaTime);
		}

		public void AddVelocityRelativeToForward(Vector2 directionVector)
		{
			Vector3 forward = GetForwardVector(out Vector3 right);

			Vector3 prelimMove = (forward * directionVector.y) + (right * directionVector.x);
			
			AddVelocity(prelimMove * Time.deltaTime);
		}


		public void AddVelocityRelativeToForward()
		{
			AddVelocityRelativeToForward(Vector2.up);
		}

		// public void SetForwardVelocity(Vector2 magnitudeVector)
		// {
		// 	velocity.x = magnitudeVector.x;
		// 	velocity.z = magnitudeVector.y;
		// }
		// public void SetForwardVelocity(float x, float z)
		// {
		// 	velocity.x = x;
		// 	velocity.z = z;
		// }

		private void Move(Vector3 moveVector)
		{
			if (moveVector.magnitude <= 0.01f) return;
			transform.position += moveVector;
		}


		public void Rotate(float rotation)
		{
			transform.Rotate(new Vector3(0, rotation, 0));
		}

		public void Jump()
		{
			if (!isGrounded) return;
			velocity.y += jumpHeight;
		}

		public void RotateTo(float positionY)
		{
			transform.rotation = Quaternion.Euler(0, positionY, 0);
		}
	}
}