using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public abstract class ItemLinearAnimatedPickups : BaseItemHoldProcessor
    {
        private bool doNotAttract;
        [field: SerializeField, Range(0.1f, 1)] public float animSpeed { get; private set; }
        public override void AttractItemToHands(PlayerHoldingData data)
        {
            if (!doNotAttract) AttrackItemWhenHolding(data);
        }
        public override void UpdateHoldingPos(PlayerHoldingData data)
        {
            if (!doNotAttract) UpdateHoldingPosWhenHolding(data);
        }
        protected abstract void AttrackItemWhenHolding(PlayerHoldingData data);
        protected abstract void UpdateHoldingPosWhenHolding(PlayerHoldingData data);
        public override IEnumerator OnBeginHold(PlayerHoldingData data)
        {
            transform.SetParent(null);
            doNotAttract = true;
            data.leftHand.target.isArmDesired = true;
            data.leftHand.target.targetArmPos = transform.position;
            data.leftHand.target.targetArmRot = transform.rotation;
            data.leftHand.DrasticalSet(data.leftHand.target);

            yield return TransferTo(data, new ItemDestinationDescription(data.leftHand.CurrentHandPos, data.leftHand.CurrentHandRotation * Vector3.forward));
            data.leftHand.DrasticalSet(data.leftHand.target);
            doNotAttract = false;
        }
        private IEnumerator TransferTo(PlayerHoldingData data, ItemDestinationDescription destination)
        {
            float T = animSpeed;
            Vector3 previousPos = transform.localPosition;
            Vector3 targetPos = destination.position;
            if (destination.newParent)
            {
                targetPos = destination.newParent.worldToLocalMatrix.MultiplyPoint3x4(destination.position);
            }
            Quaternion previousRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(destination.normal);
            while (T > 0)
            {
                float lerp = 1 - T / animSpeed;
                lerp = Mathf.Sqrt(lerp);
                transform.localPosition = Vector3.Lerp(previousPos, targetPos, lerp);
                transform.rotation = Quaternion.Lerp(previousRot, targetRot, lerp);
                data.leftHand.target.isArmDesired = true;
                data.leftHand.target.targetArmPos = transform.position;
                data.leftHand.target.targetArmRot = transform.rotation;
                yield return new WaitForEndOfFrame();
                T -= Time.deltaTime;
            }
            transform.localPosition = targetPos;
            transform.rotation = targetRot;
            data.leftHand.target.isArmDesired = false;
        }
        public override IEnumerator OnStopHold(PlayerHoldingData data, ItemDestinationDescription destination)
        {
            transform.SetParent(destination.newParent);
            doNotAttract = true;
            yield return TransferTo(data, destination);
            doNotAttract = false;
        }
    }
}