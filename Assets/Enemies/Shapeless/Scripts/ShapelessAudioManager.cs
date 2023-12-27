using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.Shapeless
{
    [RequireComponent(typeof(AudioSource))]
    public class ShapelessAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip calmingDownClip;
        [SerializeField] private AudioClip gettingAngryClip;
        private AudioSource mainAudioSource;
        private void Awake()
        {
            mainAudioSource = GetComponent<AudioSource>();
        }
        public void OnStateStarted(ShapelessState state)
        {
            switch (state)
            {
                case ShapelessState.CalmingDown:
                    mainAudioSource.PlayOneShot(calmingDownClip);
                    break;
                case ShapelessState.GettingAngry:
                    mainAudioSource.PlayOneShot(gettingAngryClip);
                    break;
            }
        }
    }
}
