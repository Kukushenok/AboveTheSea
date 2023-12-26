using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.Shapeless
{
    public class ShapelessMovingScript : MonoBehaviour
    {
        [SerializeField] private Vector3 speedDir;
        [SerializeField] private ShapelessMonsterFeatureAnimator animator;

        public void Update()
        {
            transform.Translate(speedDir * animator.speedMultiplier * Time.deltaTime);
        }
    }
}