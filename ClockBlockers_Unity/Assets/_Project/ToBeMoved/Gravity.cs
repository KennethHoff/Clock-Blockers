using Between_Names.Property_References;

using ClockBlockers.Characters;
using ClockBlockers.Utility;

using UnityEngine;


namespace ClockBlockers.ToBeMoved
{
    public class Gravity : MonoBehaviour
    {
        [SerializeField]
        private FloatReference gravityValue;

        private CharacterMovement _characterMovement;

        private void Awake()
        {
            _characterMovement = GetComponent<CharacterMovement>();
            if (_characterMovement == null)
            {
                Logging.LogIncorrectInstantiation("Character Movement", this);
                enabled = false;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (_characterMovement.isGrounded) return;
            
            _characterMovement.Velocity += new Vector3(0, gravityValue * Time.deltaTime, 0);
        }
    }
}
