using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerItemPocket : MonoBehaviour, IItemInteractResponder
    {
        [SerializeField] private PocketFitCategory pocketFit;
        private ItemBehaviour storedItem;
        public bool holdingItem { get; private set; }

        private void Manager_OnPickedUpEvent()
        {
            storedItem.OnPickedUpEvent -= Manager_OnPickedUpEvent;
            storedItem = null;
            //interactionCollider.enabled = true;
        }
        public bool OnInteracted(PlayerHoldingItemScript manager)
        {
            if (manager.ItemInfo == null && storedItem)
            {
                storedItem.OnInteracted(manager);
                return true;
            }
            if (manager.ItemInfo != null && !storedItem)
            {
                if (manager.ItemInfo.pocketFitCategory == pocketFit)
                {
                    manager.PlaceItem(new ItemDestinationDescription(transform.position, transform.forward, transform));
                    //interactionCollider.enabled = false;
                    storedItem = manager.HoldingItem;
                    storedItem.OnPickedUpEvent += Manager_OnPickedUpEvent;
                }
                return true;
            }
            return false;
        }
    }

}