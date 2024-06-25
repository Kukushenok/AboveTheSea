using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCharacterController : MonoBehaviour
{
    protected Rigidbody rg;
    [SerializeField] protected Vector3 desiredVelocity;
    [SerializeField] protected Vector3 velocity => rg.velocity;
    [SerializeField]
    [Range(0, 1)] protected float speedDamp;
    protected virtual void Awake()
    {
        rg = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        UpdateRigidbodyVelocity();
    }
    protected void Jump(float yVelocity)
    {
        rg.velocity += Vector3.up * yVelocity;
    }
    protected void UpdateRigidbodyVelocity()
    {
        float t = LerpFunctions.LerpTFixedTime(speedDamp);
        Vector3 velocity = rg.velocity;
        velocity.x = Mathf.Lerp(velocity.x, desiredVelocity.x, t);
        velocity.z = Mathf.Lerp(velocity.z, desiredVelocity.z, t);
        rg.velocity = velocity;
    }
}
