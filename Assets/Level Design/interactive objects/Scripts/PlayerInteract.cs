using UnityEngine;
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

        


        private void Awake()
        {
            
            if (_camera == null) { Debug.LogError("no camera transform"); }
            interactionReference.action.performed += OnInteraction;
        }

        private void Update()
        {
            Debug.DrawRay(_camera.position, _camera.forward);
        }

        void OnInteraction(InputAction.CallbackContext context)
        {
            RaycastHit hit;

            

            if (Physics.Raycast(_camera.position, _camera.forward, out hit, _rayLenght, _rayMask)) {
               Debug.Log("I tried");
                if (hit.collider.gameObject.TryGetComponent(out ButtonObject obj)) {
                    Debug.Log("I tried 2");
                    obj.OnInteract(transform.position);
                }
            }

        }


    }
}
