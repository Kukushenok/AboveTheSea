using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCore : MonoBehaviour
    {
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        [field: SerializeField] public PlayerInteractionManager InteractionManager { get; private set; }
        [field:SerializeField] public PlayerLookRaycaster LookRaycaster { get; private set; }
        [field: SerializeField] public PlayerItemManagerScript ItemManager { get; private set; }
    }
}