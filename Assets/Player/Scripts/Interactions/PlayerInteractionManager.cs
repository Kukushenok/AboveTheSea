using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        [SerializeField] private PlayerCore playerCore;
        [SerializeField] private InputActionReference interactionReference;
        [SerializeField] private InputActionReference leftButtonReference;
        [SerializeField] private InputActionReference rightButtonReference;

        [SerializeField] private PlayerItemManagerScript _holdingItemScript;
        private List<Interactor> interactors;
        // Start is called before the first frame update
        void Awake()
        {
            interactionReference.action.performed += OnInteraction;
            leftButtonReference.action.performed += OnLeftClick;
            rightButtonReference.action.performed += OnRightClick;
            interactors = new List<Interactor>()
            {
                new HolderGeneralInteractor(_holdingItemScript),
                new ButtonInteractor(transform),
                new DropItemInteractor(_holdingItemScript)
            };
        }

        private void OnLeftClick(InputAction.CallbackContext context)
        {
            if (playerCore.ItemManager.HoldingItem)
            {
                playerCore.ItemManager.HoldingItem.OnLeftClickInteraction(playerCore, context);
            }
        }
        private void OnRightClick(InputAction.CallbackContext context)
        {
            if (playerCore.ItemManager.HoldingItem)
            {
                playerCore.ItemManager.HoldingItem.OnRightClickInteraction(playerCore, context);
            }
        }

        void OnInteraction(InputAction.CallbackContext context)
        {
            if (playerCore.LookRaycaster.GetInteractionHit(out RaycastHit hit))
            {
                bool overriden = false;
                foreach (Interactor it in interactors)
                {
                    it.InteractionHit(hit, ref overriden);
                    if (overriden) break;
                }
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {
            interactionReference.action.performed -= OnInteraction;
        }
    }
}