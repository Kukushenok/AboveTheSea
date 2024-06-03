using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeathScreen
{
    public class DeathAnimationBehaviour : MonoBehaviour
    {
        private static int LastSceneIndex;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float animTime;
        [SerializeField] private int mainScene;
        public void PlayAnimEvent()
        {
            audioSource.Play();
            Invoke("Revive", animTime);
        }
        public void Revive()
        {
            SceneLoadTransitions.LoadScene(LastSceneIndex);
        }
        public static void ShowImmediateDeathScreen(int deathScreenIndex)
        {
            LastSceneIndex = SceneManager.GetSceneAt(0).buildIndex;
            SceneLoadTransitions.LoadSceneImmediate(deathScreenIndex);
        }
    }
}