using Item;
using Player;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

public class KeyLockBehaviour : MonoBehaviour//, IItemInteractResponder
{
    [SerializeField] private string Key;
    [SerializeField] private Transform picklock;
    [SerializeField] private UnityEvent OnUnlock;
    private ItemBehaviour currentKey;
    //public bool OnInteracted(PlayerItemManagerScript manager)
    //{
    //    if (!enabled) return false;
    //    if (manager.HoldingItem is KeyItemScript)
    //    {
    //        KeyItemScript keyItemScript = (KeyItemScript)manager.HoldingItem;
    //        if (keyItemScript.Key == Key)
    //        {
    //            currentKey = keyItemScript;
    //            StartCoroutine(WaitAndKill(manager.DisplaceItemCoroutine(
    //                new ItemDestinationDescription(picklock.transform.position, picklock.transform.forward, picklock))));
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    public bool KeyAccept(PlayerItemManagerScript manager, string key)
    {
        if (key == Key)
        {
            currentKey = manager.HoldingItem;
            StartCoroutine(WaitAndKill(manager.DisplaceItemCoroutine(
                new ItemDestinationDescription(picklock.transform.position, picklock.transform.forward, picklock))));
            return true;
        }
        return false;
    }
    IEnumerator WaitAndKill(IEnumerator deholding)
    {
        yield return deholding;
        Destroy(currentKey); // не объект, а скрипт! он становится пустышкой))
        OnUnlock.Invoke();
        enabled = false;
        GetComponent<Collider>().enabled = false; // тяжело...
    }
}
