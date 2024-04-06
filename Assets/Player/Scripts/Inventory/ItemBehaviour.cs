using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public struct TargetArmState
    {
        public Vector3 targetArmPos;
        public Quaternion targetArmRot;
        public bool isArmDesired;
    }
    public abstract class ItemHoldProtocol
    {
        public void SetupTransformPosByHandPos()
        {

        }
        public void SetupHandPosNormal()
        {

        }
        public void SetupHandPosLooking()
        {

        }
    }
    public abstract class ItemBehaviour : MonoBehaviour
    {
        [SerializeField] private ItemScriptableObject bindedScriptableObject;
        public bool isHolding { get; private set; }
        public abstract void UpdateHoldingPos(PlayerHoldingData data);
        public abstract void AttractItemToHands(PlayerHoldingData data);
        protected abstract IEnumerator OnBeginHold(PlayerHoldingData data);
        protected abstract IEnumerator OnStopHold(PlayerHoldingData data, Vector3 endPos); // Might update later
        public IEnumerator BeginHoldProcess(PlayerHoldingData data)
        {
            yield return OnBeginHold(data);
            isHolding = true;
        }
        public IEnumerator EndHoldingProcess(PlayerHoldingData data)
        {
            isHolding = false;
            yield return OnBeginHold(data);
        }
    }
}