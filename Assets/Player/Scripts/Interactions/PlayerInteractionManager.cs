using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        [SerializeField] private PlayerLookRaycaster raycaster;
        [SerializeField] private InputActionReference interactionReference;
        [SerializeField] public List<Interactor> interactors;
        [SerializeField] private PlayerItemManagerScript _holdingItemScript;
        // Start is called before the first frame update
        void Awake()
        {
            interactionReference.action.performed += OnInteraction;
            interactors = new List<Interactor>()
            {
                new HolderGeneralInteractor(_holdingItemScript),
                new ButtonInteractor(transform),
                new DropItemInteractor(_holdingItemScript)
            };
        }
        void OnInteraction(InputAction.CallbackContext context)
        {
            if (raycaster.GetInteractionHit(out RaycastHit hit))
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