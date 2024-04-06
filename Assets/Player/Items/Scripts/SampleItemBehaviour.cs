using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Item
{
    public class SampleItemBehaviour : ItemBehaviour
    {
        [SerializeField] private Vector3 delta;
        [SerializeField] private Vector3 bodyDeltaOnLow;
        private bool wasLow;
        public override void UpdateHoldingPos(PlayerHoldingData data)
        {
            data.leftHand.target.isArmDesired =true;
            Vector3 qrot = data.cameraRotation;
            
            if (data.cameraRotation.x >= 80)
            {
                qrot.x = 0;
                Quaternion rot = Quaternion.Euler(qrot);
                data.leftHand.target.targetArmPos = data.bodyPosition + rot * bodyDeltaOnLow;
                data.leftHand.target.targetArmRot = rot;
            }
            else
            {
                qrot.x /= 1.2f;
                qrot.y = Mathf.DeltaAngle(data.bodyXRotation, qrot.y) / 1.5f + data.bodyXRotation;
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
        public override void AttractItemToHands(PlayerHoldingData data)
        {
            transform.position = data.leftHand.CurrentHandPos;
            transform.rotation = data.leftHand.CurrentHandRotation;
        }

        protected override IEnumerator OnBeginHold(PlayerHoldingData data)
        {
            data.leftHand.target.targetArmPos = transform.position;
            data.leftHand.target.targetArmRot = transform.rotation;
            data.leftHand.DrasticalSet(data.leftHand.target);
            yield return new WaitForSeconds(1);
        }

        protected override IEnumerator OnStopHold(PlayerHoldingData data, Vector3 endPos)
        {
            data.leftHand.target.targetArmPos = endPos;
            data.leftHand.target.targetArmRot = transform.rotation;
            data.leftHand.DrasticalSet(data.leftHand.target);
            yield return new WaitForSeconds(1);
        }
    }
}
