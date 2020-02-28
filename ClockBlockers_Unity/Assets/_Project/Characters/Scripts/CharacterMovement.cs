using Between_Names.Property_References;

using UnityEngine;


namespace ClockBlockers.Characters
{
	public class CharacterMovement : MonoBehaviour
	{
		private CharacterController _characterController;

		[SerializeField]
		private FloatReference moveSpd;
		
		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
		}

		public void MoveForward(Vector2 vector)
		{
			Transform transform1 = transform;
			Vector3 forward = transform1.forward;
			Vector3 right = transform1.right;
			forward.y = 0;
			right.y = 0;
			forward.Normalize();
			right.Normalize();

			Vector3 prelimMove = (forward * vector.y) + (right * vector.x);
			Vector3 moveVector = prelimMove * moveSpd;

			Move(moveVector);
		}

		private void Move(Vector3 moveVector)
		{
			_characterController.Move(moveVector);
		}


		public void Rotate(float rotation)
		{
			transform.Rotate(new Vector3(0, rotation, 0));
		}

	}
}