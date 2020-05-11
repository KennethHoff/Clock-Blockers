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

        private CharacterMovementNew _characterMovement;

        private void Awake()
        {
            _characterMovement = GetComponent<CharacterMovementNew>();

            if (!Logging.CheckIfCorrectMonoBehaviourInstantiation(ref _characterMovement, this, "Character Movement"))
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (_characterMovement.IsGrounded) return;
            
            _characterMovement.AddVelocity(new Vector3(0, gravityValue * Time.deltaTime, 0) );
        }
    }
}
