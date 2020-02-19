using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletController : MonoBehaviour
{
    private Rigidbody rb;
    [ReadOnly] public int creatorInstanceID;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.GetComponent<BaseController>()) return;
        Debug.Log("Bullet collided!");

        Destroy(rb);

        transform.parent = collision.transform;
    }
}
