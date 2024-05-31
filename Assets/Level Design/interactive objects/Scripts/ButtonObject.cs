using System;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    // put this on object you want to click on to do something
    public event Action<Vector3> Interacted;
    public bool disableInteraction;
    public virtual void OnInteract(Vector3 interactorPosition) {
        if (disableInteraction) return;
        Debug.Log("button object on interact");
        Interacted?.Invoke(interactorPosition);
    }

    public void EnableInteraction() => disableInteraction = false;
    public void DisableInteraction() => disableInteraction = true;
}
