using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerLookRaycaster : MonoBehaviour
    {
        // raycast params
        [SerializeField] Transform _camera;
        [SerializeField] float _rayLenght;
        [SerializeField] LayerMask _rayMask;
        private void Awake()
        {
            if (_camera == null) { Debug.LogError("no camera transform"); }
        }
        public bool GetInteractionHit(out RaycastHit hit) => Physics.Raycast(_camera.position, _camera.forward, out hit, _rayLenght, _rayMask);


    }
}
