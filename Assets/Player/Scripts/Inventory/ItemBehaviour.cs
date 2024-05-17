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
    public struct ItemDestinationDescription
    {
        public Vector3 position;
        public Vector3 normal;
        public Transform newParent;
        public ItemDestinationDescription(RaycastHit hitInfo)
        {
            position = hitInfo.point;
            normal = hitInfo.normal;
            newParent = null;
        }
        public ItemDestinationDescription(Vector3 position)
        {
            this.position = position;
            normal = Vector3.forward;
            newParent = null;
        }
        public ItemDestinationDescription(Vector3 position, Vector3 normal, Transform newParent = null)
        {
            this.position = position;
            this.normal = normal;
            this.newParent = newParent;
        }
    }
    public abstract class ItemBehaviour : MonoBehaviour
    {
        [field: SerializeField] public ItemScriptableObject bindedScriptableObject { get; private set; }
        public bool isHolding { get; private set; }
        public abstract void UpdateHoldingPos(PlayerHoldingData data);
        public abstract void AttractItemToHands(PlayerHoldingData data);
        protected abstract IEnumerator OnBeginHold(PlayerHoldingData data);
        protected abstract IEnumerator OnStopHold(PlayerHoldingData data, ItemDestinationDescription destination); // Might update later
        public IEnumerator BeginHoldProcess(PlayerHoldingData data)
        {
            yield return OnBeginHold(data);
            isHolding = true;
        }
        public IEnumerator EndHoldingProcess(PlayerHoldingData data, ItemDestinationDescription destination)
        {
            isHolding = false;
            yield return OnStopHold(data, destination);
        }
    }
}