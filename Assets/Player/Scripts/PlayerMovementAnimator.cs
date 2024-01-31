using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovementAnimator : MonoBehaviour
{
    private const string PARAM_SPEED_FORWARD = "speed_forward";
    private const string PARAM_SPEED_SIDEWAYS = "speed_sideways";
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void SetMovementParams(Vector3 velocity, float bodyAngle)
    {
        Vector3 forwardVelocity = Quaternion.Euler(0, -bodyAngle, 0) * velocity;
        Vector2 forward2D = new Vector2(forwardVelocity.x, forwardVelocity.z);
        if (forward2D.magnitude > 1) forward2D.Normalize();
        anim.SetFloat(PARAM_SPEED_SIDEWAYS, forward2D.x);
        anim.SetFloat(PARAM_SPEED_FORWARD, forward2D.y);
    }
}
