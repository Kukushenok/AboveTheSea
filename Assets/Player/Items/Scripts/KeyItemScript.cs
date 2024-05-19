using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Item
{
    public class KeyItemScript : SampleItemBehaviour
    {
        [field: SerializeField] public string Key { get; private set; }
    }
}