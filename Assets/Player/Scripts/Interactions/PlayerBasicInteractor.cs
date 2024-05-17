using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerBasicInteractor: MonoBehaviour
    {
        public void OnInteractionHit(RaycastHit hit)
        {
            if (hit.collider.gameObject.TryGetComponent(out ButtonObject obj))
            {
                obj.OnInteract(transform.position);
            }
        }
    }
}