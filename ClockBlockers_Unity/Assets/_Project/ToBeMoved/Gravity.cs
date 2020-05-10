using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.ToBeMoved
{
    [BurstCompile]
    public class Gravity : MonoBehaviour
    {
        [SerializeField]
        private FloatReference gravityValue = null;

        // private CharacterController characterController;

        private CharacterMovementNew characterMovement;

        private void Awake()
        {
            characterMovement = GetComponent<CharacterMovementNew>();

            if (!Logging.CheckIfCorrectMonoBehaviourInstantiation(ref characterMovement, this, "Character Movement"))
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (characterMovement.IsGrounded) return;
            characterMovement.AddVelocity(new Vector3(0, gravityValue, 0) * Time.deltaTime);
        }
    }
}
