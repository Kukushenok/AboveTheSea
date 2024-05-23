using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class ShapelessKillerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.gameObject.TryGetComponent(out PlayerKillManager killManager))
            {
                killManager.KillMe();
            }
        }
    }
}
