using Item;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemInteractResponder
{
    bool OnInteracted(PlayerHoldingItemScript manager);
}
