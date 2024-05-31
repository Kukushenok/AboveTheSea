using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class ItemSoundScript : MonoBehaviour
    {
        [SerializeField] private RandomAudioSource randomAudioSource;
        private void Awake()
        {
            ItemBehaviour beh = GetComponent<ItemBehaviour>();
            beh.OnDroppedEvent += ItemStatusChanded;
            beh.OnPickedUpEvent += ItemStatusChanded;
        }

        private void ItemStatusChanded()
        {
            randomAudioSource.PlayRandomSound();
        }
    }
}