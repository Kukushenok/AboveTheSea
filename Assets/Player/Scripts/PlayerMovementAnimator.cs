using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerMovementAnimator : MonoBehaviour
    {
        private const string PARAM_SPEED_FORWARD = "speed_forward";
        private const string PARAM_SPEED_SIDEWAYS = "speed_sideways";
        private const string PARAM_IS_GROUNDED = "grounded";
        public bool isGrounded {
            get { return anim.GetBool(PARAM_IS_GROUNDED); }
            set { anim.SetBool(PARAM_IS_GROUNDED, value); }
        }
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        public void SetMovementParams(Vector3 velocity, float bodyAngle)
        {
            Vector3 forwardVelocity = Quaternion.Euler(0, -bodyAngle, 0) * velocity;
            Vector2 forward2D = new Vector2(forwardVelocity.x, forwardVelocity.z);
            anim.SetFloat(PARAM_SPEED_SIDEWAYS, forward2D.x);
            anim.SetFloat(PARAM_SPEED_FORWARD, forward2D.y);
        }
    }
}