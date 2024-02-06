using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

namespace Player
{
    public static class LerpFunctions
    {
        public static float LerpTFixedTime(float dampCoeff)
        {
            return 1 - Mathf.Pow(dampCoeff, Time.fixedDeltaTime);
        }
        public static void DampByFixedTime(ref float curr, float target, float dampCoeff)
        {
            curr = Mathf.Lerp(curr, target, LerpTFixedTime(dampCoeff));
        }
        public static void DampAngleByFixedTime(ref float curr, float target, float dampCoeff)
        {
            curr = Mathf.LerpAngle(curr, target, LerpTFixedTime(dampCoeff));
        }

        public static void DampByFixedTime(ref Vector2 vec, Vector2 target, float dampCoeff)
        {
            DampByFixedTime(ref vec.x, target.x, dampCoeff);
            DampByFixedTime(ref vec.y, target.y, dampCoeff);
        }
    }
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovementAnimator movementAnimator;
        [SerializeField]
        private Transform rotationPointTransform, cameraTransform;
        [Header("Управление")]
        [SerializeField]
        private InputActionReference movementReference, rotationReference, jumpReference, runReference;
        [Header("Параметры передвижения")]
        [SerializeField]
        private float speed, backwardsSpeed, runningSpeed, cameraSensivity;
        [SerializeField, Tooltip("Тело поворачивается вместе с камерой, если между ними угол, больший данного.")]
        private float criticalBodyRotDegrees;
        [SerializeField]
        [Range(0, 1)] private float rotationDamp = 0.1f;
        [SerializeField]
        [Range(0, 1)] private float speedDamp = 0.05f;
        [SerializeField]
        private float jumpMaxCoyoteTime = 0.4f, jumpVelocity = 4;
        CharacterController controller;
        Vector2 currentRotation;
        float currentBodyXRotation;
        Vector3 velocity;
        float currCoyoteTime;
        // Start is called before the first frame update
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            jumpReference.action.performed += OnJumpButtonPressed;
        }
        void UpdateVelocity()
        {
            Vector2 playerControl = movementReference.action.ReadValue<Vector2>();
            if (playerControl.sqrMagnitude > 1) playerControl.Normalize();
            if (runReference.action.IsPressed())
            {
                if (playerControl.y < 0)
                {
                    playerControl.y *= backwardsSpeed;
                }
                else playerControl.y *= runningSpeed;
                playerControl.x *= runningSpeed;
            }
            else
            {
                if (playerControl.y < 0)
                {
                    playerControl.y *= backwardsSpeed;
                }
                else playerControl.y *= speed;
                playerControl.x *= speed;
            }
            Quaternion rotation = Quaternion.Euler(0, currentRotation.x, 0);
            Vector3 desiredMoveVector = rotation * new Vector3(playerControl.x, 0, playerControl.y);
            LerpFunctions.DampByFixedTime(ref velocity.x, desiredMoveVector.x, speedDamp);
            LerpFunctions.DampByFixedTime(ref velocity.z, desiredMoveVector.z, speedDamp);
            if (controller.isGrounded && velocity.y < 0) velocity.y = 0;
            velocity += Physics.gravity * Time.fixedDeltaTime; // SCUFFED
            //if (!controller.isGrounded) velocity += Physics.gravity * Time.fixedDeltaTime;
            
            LerpBodyRotation();
            movementAnimator.SetMovementParams(velocity, currentBodyXRotation);
        }
        void LerpBodyRotation()
        {
            float dampAmplification = Mathf.Lerp(1, rotationDamp, new Vector2(velocity.x, velocity.z).magnitude / speed);
            currentBodyXRotation = Mathf.Repeat(currentBodyXRotation, 360);
            LerpFunctions.DampAngleByFixedTime(ref currentBodyXRotation, currentRotation.x, dampAmplification);
        }
        void UpdateBodyRotation()
        {

            float delta = Mathf.DeltaAngle(currentBodyXRotation, currentRotation.x);
            if (Mathf.Abs(delta) > criticalBodyRotDegrees)
            {
                delta = delta - Mathf.Sign(delta) * criticalBodyRotDegrees;
                currentBodyXRotation += delta;
                transform.Rotate(new Vector3(0, delta, 0));
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, currentBodyXRotation, 0));
        }
        void UpdatePosition()
        {
            CollisionFlags flags = controller.Move(velocity * Time.fixedDeltaTime);
            //velocity = controller.velocity;
        }
        void UpdateRotation()
        {
            Vector2 deltaAngle = rotationReference.action.ReadValue<Vector2>();
            float scale = 2.0f / (Screen.width + Screen.height);
            currentRotation += deltaAngle * cameraSensivity * scale;
            UpdateBodyRotation();
            currentRotation.y = Mathf.Clamp(currentRotation.y, -100.0f, 90.0f);
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360.0f);

            Vector3 rotationAngles = new Vector3(-currentRotation.y, currentRotation.x, 0);
            rotationPointTransform.rotation = Quaternion.Euler(rotationAngles);
        }

        void JumpUpdate()
        {
            if (!controller.isGrounded && currCoyoteTime > 0)
            {
                currCoyoteTime -= Time.fixedDeltaTime;
            }
            else if (controller.isGrounded) currCoyoteTime = jumpMaxCoyoteTime;
            movementAnimator.isGrounded = currCoyoteTime > 0;
        }

        void OnJumpButtonPressed(InputAction.CallbackContext context)
        {
            if (currCoyoteTime > 0)
            {
                velocity += Vector3.up * jumpVelocity;
                currCoyoteTime = 0;
            }
        }
        void FixedUpdate()
        {
            JumpUpdate();
            UpdateVelocity();

            UpdatePosition();
            UpdateRotation();
        }
        private void LateUpdate()
        {
            cameraTransform.rotation = rotationPointTransform.rotation;
        }
    }
}