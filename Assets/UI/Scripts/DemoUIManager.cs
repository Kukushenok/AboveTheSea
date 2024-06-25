using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class DemoUIManager : MonoBehaviour
    {
        public static bool SHOW_PLAYGROUND = false;
        [SerializeField] private Button playgroundButton;
        public void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            playgroundButton.interactable = SHOW_PLAYGROUND;
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
}