using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.Shapeless
{
    public enum ShapelessState { SlideNormal = 0, GettingAngry = 1, SlideAngry = 2, CalmingDown = 3 };
    public class ShapelessAnimatorStateMashine : StateMachineBehaviour
    {
        
        public delegate void CallbackOnEnter(Animator anim, ShapelessState state);
        private CallbackOnEnter currentCallback = null;
        public ShapelessState correspondingStateToMe;
        public static void SetupCallingFor(Animator anim, CallbackOnEnter callbackOnEnter)
        {
            foreach (ShapelessAnimatorStateMashine statem in anim.GetBehaviours<ShapelessAnimatorStateMashine>())
            {
                statem.currentCallback += callbackOnEnter;
            }
        }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo state, int layerIndex)
        {
            currentCallback?.Invoke(animator, correspondingStateToMe);
        }
    }
}
