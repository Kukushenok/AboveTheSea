using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoUIManager : MonoBehaviour
{
    public static bool SHOW_PLAYGROUND = false;
    [SerializeField] private GameObject playgroundButton;
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        playgroundButton.SetActive(SHOW_PLAYGROUND);
    }
    public void PlayButton()
    {
        SceneLoadTransitions.LoadScene(SceneLoadTransitions.SCENE_GAMEPLAY);
    }
    public void PlayPlaygroundButton()
    {
        SceneLoadTransitions.LoadScene(SceneLoadTransitions.SCENE_PLAYGROUND);
    }
}
