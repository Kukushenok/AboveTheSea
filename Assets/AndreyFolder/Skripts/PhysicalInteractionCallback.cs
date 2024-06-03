using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalInteractionCallback : MonoBehaviour
{
    public UnityEvent<float> OnPhysicalCollision;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        OnPhysicalCollision?.Invoke(collision.impulse.magnitude);
    }
}
