using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
namespace Player
{
    public class PlayerFootIKPlanter : MonoBehaviour
    {
        public delegate void OnStep(float velocity);

        [SerializeField] private MultiParentConstraint parentConstraint;
        [SerializeField] private Transform IKReference;
        [SerializeField] private TwoBoneIKConstraint twoBoneIKConstraint;
        [SerializeField] private Transform footTarget;
        public OnStep OnStepEvent;
        private PlayerFootManager manager;
        private float steppingTime = 0;
        private Vector3 lastFeetPos;
        public void AssignManager(PlayerFootManager manager)
        {
            this.manager = manager;
        }
        bool TryPlant(Vector3 localOffset)
        {
            Vector3 globalOffset = footTarget.rotation * localOffset;
            globalOffset.y = 0;
            Vector3 footTargetPos = footTarget.position + globalOffset;
            Vector3 rayPos = Vector3.up * manager.rayYOffset + footTargetPos;
            if (Physics.Raycast(rayPos, Vector3.down, out RaycastHit hit, manager.rayDistance, manager.mask, QueryTriggerInteraction.Ignore))
            {
                Vector3 plantLocalPos = hit.point + hit.normal * manager.plantedYOffset;
                Vector3 plantPos = plantLocalPos - Quaternion.FromToRotation(Vector3.up, hit.normal) * globalOffset;
                float angleDiff = Vector3.Angle(hit.normal, Vector3.up);
                
                Debug.DrawRay(rayPos, Vector3.down * hit.distance, Color.red);
                Debug.DrawLine(hit.point, plantPos, Color.green);
                if (angleDiff < manager.maxAngleDifference && plantLocalPos.y > footTargetPos.y)
                {
                    IKReference.position = plantPos;
                    IKReference.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * footTarget.rotation;
                    return true;
                }
            }
            return false;
        }
        public void FootIKUpdate()
        {
            parentConstraint.weight = 1f;
            bool isStepping = false;
            for (int i = 0; i < manager.forwardOffsetCount; i++)
            {
                if (TryPlant(Vector3.forward * i * manager.forwardOffset))
                {
                    isStepping = true;
                    break;
                }
            }
            twoBoneIKConstraint.weight = isStepping ? 1 : 0;
            
            if (isStepping && steppingTime < 0)
            {
                steppingTime = manager.critAudioTime;
                OnStepEvent?.Invoke((lastFeetPos.y - footTarget.position.y) / Time.smoothDeltaTime);
            }
            else if (isStepping) steppingTime = manager.critAudioTime;
            else if (!isStepping && steppingTime > 0) steppingTime -= Time.deltaTime;
            lastFeetPos = footTarget.position;
        }
    }
}