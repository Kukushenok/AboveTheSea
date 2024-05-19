using Item;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerHoldingItemScript : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        [Header("Правая рука")]
        [SerializeField] private TwoBoneIKConstraint rightHandConstraint;
        [SerializeField] private Transform rightHandTip;
        [SerializeField] private Transform rightHand;
        [Header("Левая рука")]
        [SerializeField] private TwoBoneIKConstraint leftHandConstraint;
        [SerializeField] private Transform leftHandTip;
        [SerializeField] private Transform leftHand;

        [SerializeField] private Transform body;
        [SerializeField] private Vector3 delta;
        private PlayerHoldingManager holdingManager;
        [SerializeField] private ItemBehaviour currentHoldingItem;
        [SerializeField] private float holderDampCoeff;
        public ItemScriptableObject ItemInfo
        {
            get
            {
                if (currentHoldingItem == null) return null;
                return currentHoldingItem.bindedScriptableObject;
            }
        }
        public ItemBehaviour HoldingItem => currentHoldingItem;
        public bool allowInteraction { get; private set; } = true;
        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            holdingManager = new PlayerHoldingManager
                (
                new HandManager(rightHandTip, rightHand, rightHandConstraint),
                new HandManager(leftHandTip, leftHand, leftHandConstraint),
                body, playerMovement
                );
        }

        public void Update()
        {
            if (currentHoldingItem)
            {
                currentHoldingItem.UpdateHoldingPos(holdingManager);
            }
            holdingManager.Update(holderDampCoeff);

        }
        public bool PickupItem(ItemBehaviour other)
        {
            if (currentHoldingItem != null) return false;
            currentHoldingItem = other;
            StartCoroutine(ItemChange(other.BeginHoldProcess(holdingManager)));
            return true;
        }

        public bool PlaceItem(ItemDestinationDescription destination)
        {
            if (currentHoldingItem == null) return false;
            StartCoroutine(DisplaceItemCoroutine(destination));
            return true;
        }
        public IEnumerator DisplaceItemCoroutine(ItemDestinationDescription destination)
        {
            if (currentHoldingItem == null) return null;
            return ItemDrop(currentHoldingItem.EndHoldingProcess(holdingManager, destination));
        }
        private IEnumerator ItemChange(IEnumerator other)
        {
            allowInteraction = false;
            yield return other;
            allowInteraction = true;
        }
        private IEnumerator ItemDrop(IEnumerator other)
        {
            yield return ItemChange(other);
            currentHoldingItem = null;
        }
        public void LateUpdate()
        {
            if (currentHoldingItem)
            {
                currentHoldingItem.AttractItemToHands(holdingManager);
            }
        }
    }
}