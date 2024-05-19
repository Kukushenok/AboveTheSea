using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeathScreen
{
    public class DeathAnimationBehaviour : MonoBehaviour
    {
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
            SceneManager.LoadScene(mainScene);
        }
    }
}