using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Enemies.Shapeless
{
    [RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController))]
    public class ShapelessMovingScript : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform directionObject;
        [SerializeField] private Transform rotationCore;
        [SerializeField] private float speed;
        [SerializeField] private float degreesPerSecond;
        [SerializeField] private float degreeRotateLimit;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private ShapelessMonsterFeatureAnimator animator;
        [SerializeField] private Transform targetTransformToLurk;
        private float correctionAngle = 0;
        private Vector3 targetPosition;
        private NavMeshPath path;
        private NavMeshAgent navAgent;
        private CharacterController characterController;
        private void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            characterController = GetComponent<CharacterController>();
            navAgent.updatePosition = false;
            navAgent.updateRotation = false;
        }
        private void Start()
        {
            correctionAngle = 0;
            InvokeRepeating(nameof(PathUpdate), 1, 1);
        }

        private void Update()
        {
            UpdateCorrectionAngle(targetPosition);
            Debug.DrawLine(targetPosition, targetPosition + Vector3.up, Color.yellow, 1);
            directionObject.localPosition = rotationCore.localPosition + new Vector3(Mathf.Sin(correctionAngle), 0, Mathf.Cos(correctionAngle)) * 5;
            transform.Rotate(new Vector3(0, correctionAngle * Mathf.Rad2Deg, 0) * degreesPerSecond * animator.speedMultiplier * Time.deltaTime);
            characterController.Move(transform.forward * speed * animator.speedMultiplier * Time.deltaTime);
            navAgent.nextPosition = transform.position;
        }
        private void UpdateCorrectionAngle(Vector3 desiredPos)
        {
            Vector3 delta = desiredPos - transform.position;
            float desiredToGetAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
            float error = Mathf.DeltaAngle(Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg, desiredToGetAngle);
            correctionAngle *= Mathf.Rad2Deg;
            correctionAngle = Mathf.Lerp(correctionAngle, error, 1 - Mathf.Pow(0.5f, Time.deltaTime*animator.speedMultiplier * rotationSpeed));
            correctionAngle = Mathf.Clamp(correctionAngle, -degreeRotateLimit, degreeRotateLimit);
            correctionAngle *= Mathf.Deg2Rad;
            if ((targetPosition - transform.position).magnitude < navAgent.stoppingDistance) PathUpdate();
        }

        private void PathUpdate()
        {
            if ((targetTransformToLurk.position - transform.position).magnitude < navAgent.stoppingDistance) return;
            NavMeshPath path = new NavMeshPath();
            navAgent.CalculatePath(targetTransformToLurk.position, path);
            Vector3[] pathCorners = path.corners;
            for (int i = 0; i < pathCorners.Length - 1; i++)
            {
                Debug.DrawLine(pathCorners[i], pathCorners[i + 1], Color.green, 1);
            }
            if (pathCorners.Length > 0)
            {
                targetPosition = pathCorners[0];
                int i = 1;
                while ((targetPosition - transform.position).magnitude < navAgent.stoppingDistance && i < pathCorners.Length)
                {
                    targetPosition = pathCorners[i];
                    i++;
                }
            }
        }
    }
}