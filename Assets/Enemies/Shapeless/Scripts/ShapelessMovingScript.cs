using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.Shapeless
{
    public class PIDRegulator
    {
        public delegate float ErrorFunction(float a, float b);
        public ErrorFunction errorFunc;
    }
    public class ShapelessMovingScript : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform directionObject;
        [SerializeField] private Transform rotationCore;
        [SerializeField] private float speed;
        [SerializeField] private float degreesPerSecond;
        [SerializeField] private ShapelessMonsterFeatureAnimator animator;
        private float rotationAngle = 0;
        public void Start()
        {
            rotationAngle = 0;
        }

        public void Update()
        {
            directionObject.position = rotationCore.position + new Vector3(0, Mathf.Sin(rotationAngle), Mathf.Cos(rotationAngle)) * 3;
            transform.Translate(directionObject.localPosition.normalized * speed * animator.speedMultiplier * Time.deltaTime);
            transform.Rotate(new Vector3(0, rotationAngle * Mathf.Deg2Rad, 0) * degreesPerSecond * animator.speedMultiplier * Time.deltaTime);
        }
    }
}