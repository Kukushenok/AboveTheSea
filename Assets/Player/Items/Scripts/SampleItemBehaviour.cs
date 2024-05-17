using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Item
{
    public class SampleItemBehaviour : ItemLinearAnimatedPickups
    {
        [SerializeField] private Vector3 delta;
        [SerializeField] private Vector3 bodyDeltaOnLow;
        public const float STIFFNESS_COEFF = 1.2f;
        private bool wasLow;
        protected override void UpdateHoldingPosWhenHolding(PlayerHoldingData data)
        {
            data.leftHand.target.isArmDesired =true;
            Vector3 qrot = data.cameraRotation;
            
            if (data.cameraRotation.x >= 80)
            {
                qrot.x = 0;
                qrot.y = data.bodyXRotation;
                Quaternion rot = Quaternion.Euler(qrot);
                data.leftHand.target.targetArmPos = data.bodyPosition + rot * bodyDeltaOnLow;
                data.leftHand.target.targetArmRot = rot;
            }
            else
            {
                qrot.x /= STIFFNESS_COEFF;
                qrot.y = Mathf.DeltaAngle(data.bodyXRotation, qrot.y) / STIFFNESS_COEFF + data.bodyXRotation;
                Quaternion rot = Quaternion.Euler(qrot);
                data.leftHand.target.targetArmPos = data.bodyPosition + rot * delta;
                data.leftHand.target.targetArmRot = rot;
            }
            if (wasLow != (data.cameraRotation.x < 80))
            {
                data.leftHand.DrasticalSet(data.leftHand.target);
                wasLow = data.cameraRotation.x < 80;
            }

        }
        protected override void AttrackItemWhenHolding(PlayerHoldingData data)
        {
            transform.position = data.leftHand.CurrentHandPos;
            transform.rotation = data.leftHand.CurrentHandRotation;
        }

        //protected override IEnumerator OnBeginHold(PlayerHoldingData data)
        //{
        //    transform.SetParent(null);
        //    data.leftHand.target.targetArmPos = transform.position;
        //    data.leftHand.target.targetArmRot = transform.rotation;
        //    data.leftHand.DrasticalSet(data.leftHand.target);
        //    yield return new WaitForSeconds(1);
        //}

        //protected override IEnumerator OnStopHold(PlayerHoldingData data, ItemDestinationDescription description)
        //{
        //    transform.SetParent(description.newParent);
        //    data.leftHand.target.targetArmPos = endPos;
        //    data.leftHand.target.targetArmRot = transform.rotation;
        //    data.leftHand.DrasticalSet(data.leftHand.target);
        //    yield return new WaitForSeconds(1);
        //}
    }
}
