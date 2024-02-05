using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
namespace Player
{
    public class PlayerFootIKPlanter : MonoBehaviour
    {
        [SerializeField] private MultiParentConstraint parentConstraint;
        [SerializeField] private Transform IKReference;
        [SerializeField] private TwoBoneIKConstraint twoBoneIKConstraint;
        [SerializeField] private Transform footTarget;
        // Shared.
        [SerializeField] private float rayYOffset = 1;
        [SerializeField] private float rayDistance = 0.1f;
        [SerializeField] private float plantedYOffset = 0.1f;
        [SerializeField] private LayerMask mask;
        [SerializeField] private float maxAngleDifference;
        [Header("Параметры рейкастинга")]
        [SerializeField, Tooltip("Шаг смещения лучей относительно пятки к носку")] private float forwardOffset = 0.15f;
        [SerializeField, Tooltip("Кол-во смещений лучей относительно пятки к носку")] private int forwardOffsetCount = 4;
        bool TryPlant(Vector3 localOffset)
        {
            Vector3 globalOffset = footTarget.rotation * localOffset;
            globalOffset.y = 0;
            Vector3 footTargetPos = footTarget.position + globalOffset;
            Vector3 rayPos = Vector3.up * rayYOffset + footTargetPos;
            if (Physics.Raycast(rayPos, Vector3.down, out RaycastHit hit, rayDistance, mask, QueryTriggerInteraction.Ignore))
            {
                Vector3 plantLocalPos = hit.point + hit.normal * plantedYOffset;
                Vector3 plantPos = plantLocalPos - Quaternion.FromToRotation(Vector3.up, hit.normal) * globalOffset;
                float angleDiff = Vector3.Angle(hit.normal, Vector3.up);
                
                Debug.DrawRay(rayPos, Vector3.down * hit.distance, Color.red);
                Debug.DrawLine(hit.point, plantPos, Color.green);
                if (angleDiff < maxAngleDifference && plantLocalPos.y > footTargetPos.y)
                {
                    IKReference.position = plantPos;
                    IKReference.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * footTarget.rotation;
                    return true;
                }
            }
            return false;
        }
        void LateUpdate()
        {
            parentConstraint.weight = 1f;
            float targetWeight = 0f;
            for (int i = 0; i < 4; i++)
            {
                if (TryPlant(Vector3.forward * i * forwardOffset))
                {
                    targetWeight = 1;
                    break;
                }
            }
            //if (Physics.Raycast(footTarget.position + Vector3.up * rayYOffset, Vector3.down, out RaycastHit hit, rayDistance, mask, QueryTriggerInteraction.Ignore))
            //{
            //    Vector3 plantPos = hit.point + hit.normal * plantedYOffset;
            //    if (dbgInf < maxAngleDifference && plantPos.y > footTarget.position.y)
            //    {
            //        IKReference.position = plantPos;
            //        IKReference.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * footTarget.rotation;
            //        targetWeight = 1f;
            //    }
            //    Debug.DrawRay(plantPos, Vector3.down * rayDistance, Color.red);
            //}
            twoBoneIKConstraint.weight = targetWeight;
        }
    }
}