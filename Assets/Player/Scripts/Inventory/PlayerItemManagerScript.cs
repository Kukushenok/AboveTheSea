using Item;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerItemManagerScript : ItemLogicProcessor
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
        public bool AllowInteraction { get; private set; } = true;
        public bool PickupItem(ItemBehaviour other)
        {
            if (currentHoldingItem != null) return false;
            currentHoldingItem = other;
            StartCoroutine(ItemChange(other.BeginHoldProcess(holdingManager)));
            return true;
        }
        public IEnumerator DisplaceItemCoroutine(ItemDestinationDescription destination)
        {
            if (currentHoldingItem == null) return null;
            return ItemDrop(currentHoldingItem.EndHoldingProcess(holdingManager, destination));
        }
        public bool PlaceItem(ItemDestinationDescription destination)
        {
            if (currentHoldingItem == null) return false;
            StartCoroutine(DisplaceItemCoroutine(destination));
            return true;
        }
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

        private void Update()
        {
            if (currentHoldingItem)
            {
                currentHoldingItem.UpdateHoldingPos(holdingManager);
            }
            holdingManager.Update(holderDampCoeff);

        }
        private IEnumerator ItemChange(IEnumerator other)
        {
            AllowInteraction = false;
            yield return other;
            AllowInteraction = true;
        }
        private IEnumerator ItemDrop(IEnumerator other)
        {
            yield return ItemChange(other);
            currentHoldingItem = null;
        }
        private void LateUpdate()
        {
            if (currentHoldingItem)
            {
                currentHoldingItem.AttractItemToHands(holdingManager);
            }
        }
        public override void OnLeftClick(PlayerCore core, InputAction.CallbackContext context)
        {
            if (currentHoldingItem == null) return;
            currentHoldingItem.OnLeftClickInteraction(core, context);
        }

        public override void OnRightClick(PlayerCore core, InputAction.CallbackContext context)
        {
            if (currentHoldingItem == null) return;
            currentHoldingItem.OnLeftClickInteraction(core, context);
        }
    }
}