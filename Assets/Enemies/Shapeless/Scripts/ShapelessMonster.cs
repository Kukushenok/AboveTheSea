using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
namespace Enemies.Shapeless
{
    public class ShapelessMonster : MonoBehaviour
    {
        private const string HKEY_VAR_ANGRY = "angry";
        [SerializeField] private Animator mainAnimator;
        [SerializeField] private ShapelessAudioManager myAudioManager;
        [SerializeField] private ShapelessMonsterFeatureAnimator featureAnimator;
        public bool isAngry {
            get => mainAnimator.GetBool(HKEY_VAR_ANGRY);
            set { mainAnimator.SetBool(HKEY_VAR_ANGRY, value); }
        }
        public void Awake()
        {
            ShapelessAnimatorStateMashine.SetupCallingFor(mainAnimator, OnStateChanged);
        }
        private void OnStateChanged(Animator anim, ShapelessState state)
        {
            myAudioManager.OnStateStarted(state);
        }
    }
}