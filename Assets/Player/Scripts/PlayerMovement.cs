using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform rotationPointTransform;
    [SerializeField]
    private InputActionReference movementReference, rotationReference;
    [SerializeField]
    private float speed, cameraSpeed;
    [SerializeField]
    private float criticalXDegrees;
    private Vector2 currentRotation;
    private float currentBodyXRotation;
    // Start is called before the first frame update
    void Start()
    {
        //playerInput.
    }
    void UpdatePosition()
    {
        Vector2 movement = movementReference.action.ReadValue<Vector2>();
        movement *= Time.fixedDeltaTime * speed;
        transform.Translate(movement.x, 0, movement.y);
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
        
    }
    void UpdateRotation()
    {
        Vector2 deltaAngle = rotationReference.action.ReadValue<Vector2>();
        float scale = 2.0f / (Screen.width + Screen.height);
        currentRotation += deltaAngle * cameraSpeed * scale;
        UpdateBodyRotation();
        currentRotation.y = Mathf.Clamp(currentRotation.y, -90.0f, 90.0f);
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360.0f);
        Vector3 rotationAngles = new Vector3(-currentRotation.y, currentRotation.x, 0);
        rotationPointTransform.rotation = Quaternion.Euler(rotationAngles);
        
    }
    void FixedUpdate()
    {
        UpdatePosition();
        UpdateRotation();
    }
}
