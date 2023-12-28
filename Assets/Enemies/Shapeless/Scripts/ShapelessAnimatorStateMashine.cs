using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.Shapeless
{
    public enum ShapelessState { SlideNormal = 0, GettingAngry = 1, SlideAngry = 2, CalmingDown = 3 };
    public class ShapelessAnimatorStateMashine : StateMachineBehaviour
    {
        
        public delegate void CallbackOnEnter(Animator anim, ShapelessState state, int repeatTimes);
        private CallbackOnEnter currentCallback = null;
        public ShapelessState correspondingStateToMe;
        private int repeatTimes = 0;
        public static void SetupCallingFor(Animator anim, CallbackOnEnter callbackOnEnter)
        {
            foreach (ShapelessAnimatorStateMashine statem in anim.GetBehaviours<ShapelessAnimatorStateMashine>())
            {
                statem.currentCallback += callbackOnEnter;
            }
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if ((int)stateInfo.normalizedTime > repeatTimes)
            {
                currentCallback?.Invoke(animator, correspondingStateToMe, ++repeatTimes);
            }
        }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo state, int layerIndex)
        {
            repeatTimes = 0;
            currentCallback?.Invoke(animator, correspondingStateToMe, repeatTimes);
        }
    }
}
