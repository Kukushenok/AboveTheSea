using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

namespace Player
{
    public class PlayerMovement : RigidbodyCharacterController
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
        [SerializeField]
        private GroundChecker groundChecker;
        [SerializeField, Tooltip("Тело поворачивается вместе с камерой, если между ними угол, больший данного.")]
        private float criticalBodyRotDegrees;
        [SerializeField, Tooltip("Зависимость скорости поворота тела от поворота камеры. 0 - минимальный крит. угол, 1 - максимальный.")]
        private AnimationCurve rotationDampMultiplier;
        [SerializeField, Tooltip("Критические углы вертикального наклона камеры")]
        private Vector2 cameraVerticalMinMax = new Vector2(-100, 90);
        [SerializeField]
        [Range(0, 1)] private float rotationDamp = 0.1f;
        [SerializeField]
        private float jumpMaxCoyoteTime = 0.4f, jumpVelocity = 4;
        //CharacterController controller;
        public Vector3 CurrentCameraEulerAngles => new Vector3(-_currentRotation.y, _currentRotation.x, 0);
        public float CurrentBodyXRotation => _currentBodyXRotation;
        private Vector2 _currentRotation;
        private float _currentBodyXRotation;
        float currCoyoteTime;
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            //controller = GetComponent<CharacterController>();
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
            Quaternion rotation = Quaternion.Euler(0, _currentRotation.x, 0);
            desiredVelocity = rotation * new Vector3(playerControl.x, 0, playerControl.y);
            //LerpFunctions.DampByFixedTime(ref velocity.x, desiredMoveVector.x, speedDamp);
            //LerpFunctions.DampByFixedTime(ref velocity.z, desiredMoveVector.z, speedDamp);
            //if (controller.isGrounded && velocity.y < 0) velocity.y = 0;
            //desiredVelocity += Physics.gravity * Time.fixedDeltaTime; // SCUFFED
            //if (!controller.isGrounded) velocity += Physics.gravity * Time.fixedDeltaTime;
            
            LerpBodyRotation();
            movementAnimator.SetMovementParams(velocity, _currentBodyXRotation);
        }
        void LerpBodyRotation()
        {
            float angleCor = rotationDampMultiplier.Evaluate((_currentRotation.y - cameraVerticalMinMax.x) / (cameraVerticalMinMax.y - cameraVerticalMinMax.x));
            Debug.Log(angleCor);
            float dampAmplification = Mathf.Lerp(1, rotationDamp, new Vector2(velocity.x, velocity.z).magnitude * angleCor / speed);
            _currentBodyXRotation = Mathf.Repeat(_currentBodyXRotation, 360);
            LerpFunctions.DampAngleByFixedTime(ref _currentBodyXRotation, _currentRotation.x, dampAmplification);
        }
        void UpdateBodyRotation()
        {

            float delta = Mathf.DeltaAngle(_currentBodyXRotation, _currentRotation.x);
            if (Mathf.Abs(delta) > criticalBodyRotDegrees)
            {
                delta = delta - Mathf.Sign(delta) * criticalBodyRotDegrees;
                _currentBodyXRotation += delta;
                transform.Rotate(new Vector3(0, delta, 0));
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, _currentBodyXRotation, 0));
        }
        void UpdatePosition()
        {
            UpdateRigidbodyVelocity();
            //CollisionFlags flags = controller.Move(velocity * Time.fixedDeltaTime);
            //velocity = controller.velocity;
        }
        void UpdateRotation()
        {
            UpdateBodyRotation();
            _currentRotation.y = Mathf.Clamp(_currentRotation.y, cameraVerticalMinMax.x, cameraVerticalMinMax.y);
            _currentRotation.x = Mathf.Repeat(_currentRotation.x, 360.0f);
            rotationPointTransform.rotation = Quaternion.Euler(CurrentCameraEulerAngles);
        }

        void JumpUpdate()
        {
            bool isOnGround = groundChecker.IsOnGround();
            //if (isOnGround) rg.AddForce(-Physics.gravity, ForceMode.Acceleration);
            if (!isOnGround && currCoyoteTime > 0)
            {
                currCoyoteTime -= Time.fixedDeltaTime;
            }
            else if (isOnGround) currCoyoteTime = jumpMaxCoyoteTime;
            movementAnimator.isGrounded = currCoyoteTime > 0;
        }

        void OnJumpButtonPressed(InputAction.CallbackContext context)
        {
            if (currCoyoteTime > 0)
            {
                rg.velocity += Vector3.up * jumpVelocity;
                currCoyoteTime = 0;
            }
        }
        protected override void FixedUpdate()
        {
            UpdateVelocity();
            UpdatePosition();
            UpdateRotation();
            JumpUpdate();
        }
        void UpdateRotationValue()
        {
            Vector2 deltaAngle = rotationReference.action.ReadValue<Vector2>();
            float scale = 2.0f / (Screen.width + Screen.height);
            _currentRotation += deltaAngle * cameraSensivity * scale;
        }
        public void Update()
        {
            UpdateRotationValue();
        }
        private void LateUpdate()
        {
            cameraTransform.rotation = rotationPointTransform.rotation;
        }
    }
}