using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public abstract class ItemLinearAnimatedPickups : ItemBehaviour
    {
        private bool doNotAttract;
        [SerializeField] [Range(0.1f, 1)] private float animSpeed;
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
        protected override IEnumerator OnBeginHold(PlayerHoldingData data)
        {
            transform.SetParent(null);
            doNotAttract = true;
            data.leftHand.target.isArmDesired = true;
            data.leftHand.target.targetArmPos = transform.position;
            data.leftHand.target.targetArmRot = transform.rotation;
            data.leftHand.DrasticalSet(data.leftHand.target);
            yield return new WaitForSeconds(animSpeed);
            doNotAttract = false;
        }

        protected override IEnumerator OnStopHold(PlayerHoldingData data, ItemDestinationDescription destination)
        {
            transform.SetParent(destination.newParent);
            doNotAttract = true;
            float T = animSpeed;
            Vector3 previousPos = transform.position;
            Quaternion previousRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(destination.normal);
            while(T > 0)
            {
                float lerp = 1 - T / animSpeed;
                lerp = Mathf.Sqrt(lerp);
                transform.position = Vector3.Lerp(previousPos, destination.position, lerp);
                transform.rotation = Quaternion.Lerp(previousRot, targetRot, lerp);
                data.leftHand.target.isArmDesired = true;
                data.leftHand.target.targetArmPos = transform.position;
                data.leftHand.target.targetArmRot = transform.rotation;
                yield return new WaitForEndOfFrame();
                T -= Time.deltaTime;
                
            }
            transform.position = destination.position;
            transform.rotation = Quaternion.LookRotation(destination.normal);
            data.leftHand.target.isArmDesired = false;
            doNotAttract = false;
        }
    }
}