using System.Collections;

using Between_Names.Property_References;

using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;
using UnityEngine.Serialization;


namespace ClockBlockers.Characters
{
	[BurstCompile]
	public class CharacterMovementNew : MonoBehaviour
	{
		public CharacterController characterController;

		private Vector3 inputVelocity;

		// private float _currGravityAmount = 0;

		public Vector3 currVelocity;

		private Transform charTransform;
		
		private bool isJumping;

		[FormerlySerializedAs("jumpFallOff")]
		
		[SerializeField]
		private AnimationCurve jumpCurve = null;

		public FloatReference jumpMultiplier = null;
		
		
		[SerializeField]
		private FloatReference moveSpd = null;

		public bool IsGrounded => (characterController.collisionFlags & CollisionFlags.Below) != 0;

		public bool IsHittingCeiling => (characterController.collisionFlags & CollisionFlags.Above) != 0;
		public float MoveSpd => moveSpd;
		public bool IsHittingASide => (characterController.collisionFlags & CollisionFlags.Sides) != 0;

		private void Awake()
		{
			charTransform = transform;
			
			characterController = GetComponent<CharacterController>();
			Logging.CheckIfCorrectComponentInstantiation(ref characterController, this, "Character Controller");
		}

		private void Update()
		{
			Move();

			if (!IsGrounded) return;
			currVelocity.y = -characterController.stepOffset * Time.deltaTime;
		}

		private void Move()
		{
			Vector3 finalVelocity = inputVelocity + currVelocity;
			
			characterController.Move(finalVelocity * Time.deltaTime);
			inputVelocity = Vector3.zero;
		}

		public void Rotate(float sideToSideCharacterRotation)
		{
			transform.Rotate(Vector3.up * sideToSideCharacterRotation);
		}

		public void SetForwardInputVelocity(Vector2 directionVector)
		{
			Vector3 forward = charTransform.forward;
			Vector3 right = charTransform.right;
			
			Vector3 prelimMove = (forward * directionVector.y) + (right * directionVector.x);
			
			SetInputVelocity(prelimMove * moveSpd);
		}

		public void Jump()
		{
			if (isJumping) return;
			
			isJumping = true;
			StartCoroutine(JumpRoutine());

			// AddVelocity(Vector3.up * jumpVelocity);
		}

		private IEnumerator JumpRoutine()
		{
			float currSlopeLimit = characterController.slopeLimit;
			
			characterController.slopeLimit = 90.0f;
			var timeInAir = 0.0f;
			
			do
			{
				float jumpForce = jumpCurve.Evaluate(timeInAir);
				characterController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
				
				timeInAir += Time.deltaTime;
				
				yield return null;
			} while (!IsGrounded && !IsHittingCeiling);

			isJumping = false;
			characterController.slopeLimit = currSlopeLimit;
		}

		private void SetInputVelocity(Vector3 addedVelocity)
		{
			inputVelocity = addedVelocity;
		}

		public void AddVelocity(Vector3 addedVelocity)
		{
			currVelocity += addedVelocity;
		}

		public void SetVelocityForward()
		{
			SetForwardInputVelocity(Vector2.up);
		}
	}
}