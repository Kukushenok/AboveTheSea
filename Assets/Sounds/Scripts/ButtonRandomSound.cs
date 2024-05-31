using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRandomSound : MonoBehaviour
{
    [SerializeField] private List<ButtonObject> buttonObject;
    [SerializeField] private RandomAudioSource random;
    private void Awake()
    {
        foreach(ButtonObject button in buttonObject)
        {
            button.Interacted += ButtonObject_Interacted;
        }
    }

    private void ButtonObject_Interacted(Vector3 obj)
    {
        random.PlayRandomSound();
    }
}
