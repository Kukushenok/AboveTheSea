using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class DropItemInteractor : Interactor
    {
        private PlayerHoldingItemScript playerHolder;
        public DropItemInteractor(PlayerHoldingItemScript playerHolder)
        {
            this.playerHolder = playerHolder;
        }
        public override void InteractionHit(RaycastHit hit, ref bool overrideOthers)
        {
            if (playerHolder.ItemInfo != null && playerHolder.ItemInfo.canBeDropped)
            {
                playerHolder.PlaceItem(new Item.ItemDestinationDescription(hit));
                overrideOthers = true;
            }
        }
    }
}
