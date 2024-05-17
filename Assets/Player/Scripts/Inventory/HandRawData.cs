using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEngine.GraphicsBuffer;

namespace Item
{
    public class HandRawData
    {
        public TargetArmState target;
        protected TargetArmState current;
        public Vector3 CurrentHandPos => handTransform.position;
        public Quaternion CurrentHandRotation => handTransform.rotation;
        protected Transform handTransform;
        protected float closureCoeff = 1;
        public HandRawData(Transform handTransform)
        {
            this.handTransform = handTransform;
            current = new TargetArmState() { isArmDesired = false };
            target = new TargetArmState() { isArmDesired = false };
        }
        /// <summary>
        /// Используется при РЕЗКИХ перемещениях.
        /// </summary>
        /// <param name="newTarget"></param>
        public void DrasticalSet(TargetArmState newTarget)
        {
            target = newTarget;
            closureCoeff = 0;
        }
        protected HandRawData() { }
    }
    public class HandManager: HandRawData
    {
        const float VALIDITY = 1e-3f;
        Transform handTargetTransform;
        TwoBoneIKConstraint constraint;
        public void UpdateByLerpCoeff(float lerpCoeff)
        {
            constraint.weight = Mathf.Lerp(constraint.weight, target.isArmDesired ? 1 : 0, lerpCoeff);
            if (closureCoeff >= 1 - VALIDITY)
            {
                current = target;
                closureCoeff = 1;
            }
            else
            {
                closureCoeff = Mathf.Lerp(closureCoeff, 1, lerpCoeff);
                float deqCoeff = Mathf.Pow(closureCoeff, 3);
                current.targetArmRot = Quaternion.Lerp(current.targetArmRot, target.targetArmRot, deqCoeff);
                current.targetArmPos = Vector3.Lerp(current.targetArmPos, target.targetArmPos, deqCoeff);
            }
            current.isArmDesired = target.isArmDesired;
            if (current.isArmDesired)
            {
                handTargetTransform.position = current.targetArmPos;
                handTargetTransform.rotation = current.targetArmRot;
            }
        }
        public HandManager(Transform handTargetTransform, Transform handTransform, TwoBoneIKConstraint constraint)
        {
            this.handTargetTransform = handTargetTransform;
            this.handTransform = handTransform;
            this.constraint = constraint;
            closureCoeff = 1;
            current = new TargetArmState() { isArmDesired = false };
            target = new TargetArmState() { isArmDesired = false };
        }
    }
}