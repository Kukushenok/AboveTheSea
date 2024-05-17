using System;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    // put this on object you want to click on to do something
    public event Action<Vector3> Interacted;
    public virtual void OnInteract(Vector3 interactorPosition) {
        Debug.Log("button object on interact");
        Interacted?.Invoke(interactorPosition);
    }
}
