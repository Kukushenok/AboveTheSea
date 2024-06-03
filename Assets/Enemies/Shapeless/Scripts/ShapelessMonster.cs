using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Shapeless
{
    public class ShapelessMonster : MonoBehaviour
    {
        private const string HKEY_VAR_ANGRY = "angry";
        [SerializeField] private Animator mainAnimator;
        [SerializeField] private ShapelessAudioManager myAudioManager;
        [SerializeField] private ShapelessMovingScript myMovingScript;
        [SerializeField] private ShapelessMonsterFeatureAnimator featureAnimator;
        [SerializeField] private UnityEvent OnDeath;
        public bool isAngry {
            get => mainAnimator.GetBool(HKEY_VAR_ANGRY);
            set { mainAnimator.SetBool(HKEY_VAR_ANGRY, value); }
        }
        public bool initAngry;
        public void Awake()
        {
            ShapelessAnimatorStateMashine.SetupCallingFor(mainAnimator, OnStateChanged);
            isAngry = initAngry;
        }
        private void OnStateChanged(Animator anim, ShapelessState state, int repeatCount)
        {
            myAudioManager.OnStateStarted(state, repeatCount);
        }

        public void Death()
        {
            myMovingScript.enabled = false;
            myAudioManager.Death();
            mainAnimator.Play("death");
            OnDeath?.Invoke();
        }
    }
}