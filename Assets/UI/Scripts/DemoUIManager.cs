using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoUIManager : MonoBehaviour
{
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void PlayButton()
    {
        SceneLoadTransitions.LoadScene(SceneLoadTransitions.SCENE_GAMEPLAY);
    }
}
