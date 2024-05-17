using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Item
{
    public enum PocketFitCategory
    {
        Small = 0, Medium = 1
    }
    [CreateAssetMenu(fileName = "New item", menuName = "ATS/Item", order = 0)]
    public class ItemScriptableObject : ScriptableObject
    {
        [field: SerializeField] public GameObject itemPrefab { get; private set; }
        [field: SerializeField] public bool canBeDropped { get; private set; }
        [field: SerializeField] public PocketFitCategory pocketFitCategory { get; private set; }
    }
}