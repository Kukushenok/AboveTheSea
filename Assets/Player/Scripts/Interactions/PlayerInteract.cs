using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private InputActionReference interactionReference;
        [SerializeField] Transform _camera;
        // raycast params
        [SerializeField] float _rayLenght;
        [SerializeField] LayerMask _rayMask;
        [SerializeField] public List<Interactor> interactors;
        [SerializeField] private PlayerHoldingItemScript _holdingItemScript;


        private void Awake()
        {
            
            if (_camera == null) { Debug.LogError("no camera transform"); }
            interactionReference.action.performed += OnInteraction;

            interactors = new List<Interactor>()
            {
                new HolderGeneralInteractor(_holdingItemScript),
                new ButtonInteractor(transform),
                new DropItemInteractor(_holdingItemScript)
            };
        }

        private void Update()
        {
            Debug.DrawRay(_camera.position, _camera.forward);
        }

        void OnInteraction(InputAction.CallbackContext context)
        {
            RaycastHit hit;

            if (Physics.Raycast(_camera.position, _camera.forward, out hit, _rayLenght, _rayMask)) {
                bool overriden = false;
                foreach(Interactor it in interactors)
                {
                    it.InteractionHit(hit, ref overriden);
                    if (overriden) break;
                }
            }

        }
        private void OnDestroy()
        {
            interactionReference.action.performed -= OnInteraction;
        }


    }
}
