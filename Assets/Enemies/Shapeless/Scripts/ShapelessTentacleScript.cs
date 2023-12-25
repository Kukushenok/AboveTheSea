using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Enemies.Shapeless
{
    public class ShapelessTentacleScript : MonoBehaviour
    {
        [System.Serializable]
        private struct TentacleFragment
        {
            public Transform fragmentTransform;
            public Vector3 noiseOffsets;
            public Vector3 magnitude;
            public float frequency;
            private float startTime;
            public TentacleFragment(Transform fragmentTransform, Vector3 noiseOffsets, Vector3 magnitude, float frequency)
            {
                this.fragmentTransform = fragmentTransform;
                this.noiseOffsets = noiseOffsets;
                this.magnitude = magnitude;
                this.frequency = frequency;
                startTime = Time.time;
            }
            public void ResetAnimTime()
            {
                startTime = Time.time;
            }
            public void EvaluateAnimation()
            {
                Vector3 pNoise = new Vector3();
                pNoise.x = Mathf.PerlinNoise(noiseOffsets.x, frequency * (Time.time - startTime)) * 2 - 1;
                pNoise.y = Mathf.PerlinNoise(noiseOffsets.y, frequency * (Time.time - startTime)) * 2 - 1;
                pNoise.z = Mathf.PerlinNoise(noiseOffsets.z, frequency * (Time.time - startTime)) * 2 - 1;
                fragmentTransform.SetLocalPositionAndRotation(fragmentTransform.localPosition,
                    Quaternion.Euler(Vector3.Scale(magnitude, pNoise)));
            }
        };

        public bool isAnimationActive { get { return coreRootBone.gameObject.activeInHierarchy; } }
        private const string HKEY_TENTACLE_SPEED = "speed";
        private const string HKEY_TENTACLE_SHOW_UP = "show_up";
        private const string HKEY_TENTACLE_HIDE = "hide";
        [Header("Параметры анимации появления/исчезновения тентакли")]
        [SerializeField] private Animator tentacleStatusAnimator;
        [SerializeField] private float timeForInitStatus;
        [SerializeField] private Vector2 timeForInitStatusRandSpeed;
        [SerializeField] private GameObject tentacleMeshObject;
        [Header("Параметры анимации тентакли")]
        [SerializeField] private Transform coreRootBone;
        [SerializeField] private Vector3 angleWiggleMagnitude;
        [SerializeField] private AnimationCurve wiggleMagnitudeWeight;
        [SerializeField] private AnimationCurve angleFrequency;
        private TentacleFragment[] allFragments;
        private bool hiding = false;
        private int GetCountOfTentacle(Transform root)
        {
            Transform current = root;
            int result = 0;
            while (current != null)
            {
                result++;
                if (current.childCount == 0) current = null;
                else current = current.GetChild(0);
            }
            return result;
        }
        private void Awake()
        {
            int length = GetCountOfTentacle(coreRootBone);
            allFragments = new TentacleFragment[length];
            Transform current = coreRootBone;
            int index = 0;
            while (current != null)
            {
                Vector3 noiseOffset = Random.insideUnitSphere.normalized * 30;
                allFragments[index] = new TentacleFragment(current, noiseOffset, angleWiggleMagnitude * wiggleMagnitudeWeight.Evaluate(((float)index) / length),
                    angleFrequency.Evaluate(((float)index) / length));
                if (current.childCount == 0) current = null;
                else current = current.GetChild(0);
                index++;
            }
        }
        private void Activate()
        {
            coreRootBone.gameObject.SetActive(true);
            enabled = true;
            hiding = false;
            tentacleMeshObject.SetActive(true);
            foreach (TentacleFragment fragment in allFragments) fragment.ResetAnimTime();
        }
        private void Deactivate()
        {
            coreRootBone.gameObject.SetActive(false);
            enabled = false;
            hiding = false;
            tentacleMeshObject.SetActive(false);
        }
        public void StartAnimatingWithDelay()
        {
            Invoke(nameof(StartAnimating), Random.Range(0, timeForInitStatus));
        }
        public void StartAnimating()
        {
            if (isAnimationActive || hiding) return;
            Activate();
            tentacleStatusAnimator.SetFloat(HKEY_TENTACLE_SPEED, 1 / Random.Range(timeForInitStatusRandSpeed.x, timeForInitStatusRandSpeed.y));
            tentacleStatusAnimator.Play(HKEY_TENTACLE_SHOW_UP);
        }
        public void StopAnimating()
        {
            tentacleStatusAnimator.SetFloat(HKEY_TENTACLE_SPEED, 1 / Random.Range(timeForInitStatusRandSpeed.x, timeForInitStatusRandSpeed.y));
            tentacleStatusAnimator.Play(HKEY_TENTACLE_HIDE);
            hiding = true;
        }

        private void Update()
        {
            if (!isAnimationActive)
            {
                enabled = false;
                return;
            }
            foreach (TentacleFragment fragment in allFragments)
            {
                fragment.EvaluateAnimation();
            }
        }
    }
}