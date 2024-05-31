using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class Interactor
    {
        public abstract void InteractionHit(RaycastHit hit, ref bool overrideOthers);
    }
    public class ButtonInteractor: Interactor
    {
        private Transform playerTransform;
        public ButtonInteractor(Transform playerTransform)
        {
            this.playerTransform = playerTransform;
        }
        public override void InteractionHit(RaycastHit hit, ref bool overrideOthers)
        {
            if (hit.collider.gameObject.TryGetComponent(out ButtonObject obj))
            {
                obj.OnInteract(playerTransform.position);
                overrideOthers = true;
            }
        }
    }
}