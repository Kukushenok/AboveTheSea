using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Demo
{
    public class DemoFinaleScript : MonoBehaviour
    {
        [SerializeField] private AudioSource faithJumpSource;
        [SerializeField] private AudioSource endSource;
        [SerializeField] private AudioMixerSnapshot mainSnapshot;
        [SerializeField] private AudioMixerSnapshot endSnapshot;
        [SerializeField] private GameObject endAnimator;
        [SerializeField] private float endAnimatorLength;
        public void OnPlayerJumped()
        {
            endSnapshot.TransitionTo(0.5f);
            faithJumpSource.Play();
        }
        public void WaterJumped()
        {
            endSource.Play();
            Time.timeScale = 0;
            endAnimator.gameObject.SetActive(true);
            StartCoroutine(ReturnAfter(endAnimatorLength));
        }
        private IEnumerator ReturnAfter(float seconds)
        {
            yield return new WaitForSecondsRealtime(seconds);
            ReturnToNormal();
        }
        public void ReturnToNormal()
        {
            Time.timeScale = 1;
            mainSnapshot.TransitionTo(1);
            DemoUIManager.SHOW_PLAYGROUND = true;
            SceneLoadTransitions.LoadScene(SceneLoadTransitions.SCENE_MAINMENU);
        }
        
    }
}