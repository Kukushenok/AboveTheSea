using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.Shapeless
{
    [RequireComponent(typeof(AudioSource))]
    public class ShapelessAudioManager : MonoBehaviour
    {
        [SerializeField] private ShapelessMonsterFeatureAnimator featureAnimator;
        [Header("«вуки одноразовые")]
        [SerializeField] private AudioClip calmingDownClip;
        [SerializeField] private AudioClip gettingAngryClip;
        [SerializeField] private AudioClip slideAngryClip;
        [SerializeField] private AudioClip slideCalmClip;
        [Header("Ёмбиент")]
        [SerializeField] private AudioSource ambientAudioSource;
        [SerializeField] private float ambientPitchOffset;
        [SerializeField] private float ambientPitchByFrequency;
        [SerializeField] private AudioSource slidingNormalAudioSource;
        [SerializeField] private AudioSource slidingAngryAudioSource;
        //[SerializeField] private AudioSource[] calmAmbientSources;
        [SerializeField] private AnimationCurve fadeInCurve;
        [SerializeField] private AnimationCurve fadeOutCurve;
        private AudioSource mainAudioSource;
        private void Awake()
        {
            mainAudioSource = GetComponent<AudioSource>();
        }
        public void OnStateStarted(ShapelessState state, int repeatCount)
        {
            switch (state)
            {
                case ShapelessState.SlideNormal:
                    if (repeatCount == 0) Fade(fadeInCurve, 1, slidingNormalAudioSource);
                    mainAudioSource.PlayOneShot(slideCalmClip);
                    break;
                case ShapelessState.CalmingDown:
                    Fade(fadeOutCurve, 0, slidingAngryAudioSource);
                    mainAudioSource.PlayOneShot(calmingDownClip);
                    break;
                case ShapelessState.GettingAngry:
                    Fade(fadeInCurve, 0, slidingNormalAudioSource);
                    //Fade(calmAmbientSources, calmAmbientFadeOff, 0);
                    mainAudioSource.PlayOneShot(gettingAngryClip);
                    break;
                case ShapelessState.SlideAngry:
                    if (repeatCount == 0) Fade(fadeInCurve, 1, slidingAngryAudioSource);
                    mainAudioSource.PlayOneShot(slideAngryClip);
                    break;
            }
        }
        private void ResetAudioSources(params AudioSource[] sources)
        {
            foreach (AudioSource source in sources)
            {
                source.Play();
            }
        }
        private void Fade(AnimationCurve curve, float targetVolume = 0, params AudioSource[] sources) => StartCoroutine(FadeCoroutine(sources, curve, targetVolume));
        private IEnumerator FadeCoroutine(AudioSource[] sources, AnimationCurve curve, float targetVolume = 0)
        {
            float T = 0;
            float value = curve.Evaluate(T);
            while (!Mathf.Approximately(value, targetVolume))
            {
                foreach (AudioSource source in sources) source.volume = value;
                yield return new WaitForEndOfFrame();
                T += Time.unscaledDeltaTime;
                value = curve.Evaluate(T);
            }
            foreach (AudioSource source in sources) source.volume = targetVolume;
        }
        public void Update()
        {
            ambientAudioSource.pitch = ambientPitchOffset + featureAnimator.featureFrequency * ambientPitchByFrequency;
        }
    }
}
