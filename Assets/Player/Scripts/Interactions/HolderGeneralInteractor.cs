using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class HolderGeneralInteractor : Interactor
    {
        private PlayerItemManagerScript playerHolder;
        public HolderGeneralInteractor(PlayerItemManagerScript playerHolder)
        {
            this.playerHolder = playerHolder;
        }
        public override void InteractionHit(RaycastHit hit, ref bool overrideOthers)
        {
            if (!playerHolder.AllowInteraction)
            {
                overrideOthers = true;
                return;
            }
            if (hit.collider.gameObject.TryGetComponent(out IItemInteractResponder behaviour))
            {
                behaviour.OnInteracted(playerHolder);
                overrideOthers = true;
            }
        }
    }
}