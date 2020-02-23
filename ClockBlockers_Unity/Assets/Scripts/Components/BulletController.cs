using ClockBlockers.Characters;
using ClockBlockers.Utility;
using UnityEngine;

namespace ClockBlockers.Components
{
    public class BulletController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        [HideInInspector] public int creatorInstanceId;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.transform.GetComponent<BaseController>()) return;
            Logging.Log("Bullet collided!");

            Destroy(_rigidbody);

            transform.parent = collision.transform;
        }
    }
}
