using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Item
{
    public class KeyItemScript : ItemLogicProcessor
    {
        [field: SerializeField] public string Key { get; private set; }
        public override void OnLeftClick(PlayerCore core, InputAction.CallbackContext context)
        {
            base.OnLeftClick(core, context);
            if(core.LookRaycaster.GetInteractionComponent(out KeyLockBehaviour behaviour))
            {
                behaviour.KeyAccept(core.ItemManager, Key);
            }

        }
    }
}