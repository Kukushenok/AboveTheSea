using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Item
{
    [CreateAssetMenu(fileName = "New item", menuName = "ATS/Item", order = 0)]
    public class ItemScriptableObject : ScriptableObject
    {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private bool canBeDropped;
        
    }
}