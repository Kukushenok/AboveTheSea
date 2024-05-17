using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    [RequireComponent(typeof(ButtonObject))]
    public class PlayerItemPocket : MonoBehaviour
    {
        [SerializeField] private PlayerHoldingItemScript manager;
        [SerializeField] private PocketFitCategory pocketFit;
        private ItemDestinationDescription destination;
        [SerializeField] private Collider interactionCollider;
        
        public bool holdingItem { get; private set; }
        private void Awake()
        {
            destination = new ItemDestinationDescription(transform.position, transform.forward, transform);
            GetComponent<ButtonObject>().Interacted += PlayerItemPocket_Interacted;
        }

        private void PlayerItemPocket_Interacted(Vector3 obj)
        {
            OnClick();
        }

        private void Manager_OnPickedUpEvent()
        {
            interactionCollider.enabled = true;
            manager.OnPickedUpEvent -= Manager_OnPickedUpEvent;
        }

        private void OnClick()
        {
            if (manager.ItemInfo != null)
            {
                if (manager.ItemInfo.pocketFitCategory == pocketFit)
                {
                    manager.PlaceItem(destination);
                    interactionCollider.enabled = false;
                    manager.OnPickedUpEvent += Manager_OnPickedUpEvent;
                }
            }
        }
    }

}