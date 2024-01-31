using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementAnimator movementAnimator;
    [SerializeField]
    private Transform rotationPointTransform, cameraTransform;
    [SerializeField]
    private InputActionReference movementReference, rotationReference;
    [SerializeField]
    private float speed, backwardsSpeed, cameraSpeed;
    [SerializeField]
    private float criticalXDegrees;
    [SerializeField]
    [Range(0, 1)] private float rotationDamp = 0.1f;
    [SerializeField]
    [Range(0, 1)] private float speedDamp = 0.05f;
    CharacterController controller;
    Vector2 currentRotation;
    float currentBodyXRotation;
    Vector3 velocity;
    // Start is called before the first frame update
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    float DampCoeff(float damp)
    {
        return 1 - Mathf.Pow(damp, Time.fixedDeltaTime);
    }
    void UpdateVelocity()
    {
        Vector2 movement = movementReference.action.ReadValue<Vector2>();
        movement.x *= speed;
        movement.y *= (movement.y < 0 ? backwardsSpeed : speed);
        Quaternion rotation = Quaternion.Euler(0, currentRotation.x, 0);
        Vector3 desiredMoveVector = rotation * new Vector3(movement.x, 0, movement.y);
        velocity.x = Mathf.Lerp(velocity.x, desiredMoveVector.x, DampCoeff(speedDamp));
        velocity.z = Mathf.Lerp(velocity.z, desiredMoveVector.z, DampCoeff(speedDamp));
        if (!controller.isGrounded) velocity += Physics.gravity * Time.fixedDeltaTime;
        else if (velocity.y < 0) velocity.y = 0;
        LerpBodyRotation();
        movementAnimator.SetMovementParams(velocity/speed, currentBodyXRotation);
    }
    void LerpBodyRotation()
    {
        float dampAmplification = Mathf.Lerp(1, rotationDamp, new Vector2(velocity.x, velocity.z).magnitude / speed);
        currentBodyXRotation = Mathf.LerpAngle(Mathf.Repeat(currentBodyXRotation, 360), currentRotation.x, DampCoeff(dampAmplification));
    }
    void UpdateBodyRotation()
    {
        
        float delta = Mathf.DeltaAngle(currentBodyXRotation, currentRotation.x);
        if (Mathf.Abs(delta) > criticalXDegrees)
        {
            delta = delta - Mathf.Sign(delta) * criticalXDegrees;
            currentBodyXRotation += delta;
            transform.Rotate(new Vector3(0, delta, 0));
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, currentBodyXRotation, 0));
    }
    void UpdatePosition()
    {
        controller.Move(velocity * Time.fixedDeltaTime);
    }
    void UpdateRotation()
    {
        Vector2 deltaAngle = rotationReference.action.ReadValue<Vector2>();
        float scale = 2.0f / (Screen.width + Screen.height);
        currentRotation += deltaAngle * cameraSpeed * scale;
        UpdateBodyRotation();
        currentRotation.y = Mathf.Clamp(currentRotation.y, -100.0f, 90.0f);
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360.0f);

        Vector3 rotationAngles = new Vector3(-currentRotation.y, currentRotation.x, 0);
        rotationPointTransform.rotation = Quaternion.Euler(rotationAngles);
    }
    void FixedUpdate()
    {
        UpdateVelocity();
        UpdatePosition();
        UpdateRotation();
    }
    private void LateUpdate()
    {
        cameraTransform.rotation = rotationPointTransform.rotation;
    }
}
