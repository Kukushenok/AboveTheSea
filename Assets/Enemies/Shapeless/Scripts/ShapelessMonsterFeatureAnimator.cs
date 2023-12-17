using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Shapeless
{
    public class ShapelessMonsterFeatureAnimator : MonoBehaviour
    {
        private struct FeatureAnimData
        {
            public float periodDelta;
            public float freqMutiplier;
            public FeatureAnimData(float periodDelta, float freqMutiplier)
            {
                this.periodDelta = periodDelta;
                this.freqMutiplier = freqMutiplier;
            }
            public float GetTime()
            {
                return (periodDelta + Time.time) * freqMutiplier;
            }
        }

        const string HKEY_FEATURE_FREQUENCY = "feature_frequency";
        [Header("Распределения частоты фичей по времени (от 0 до 1)")]
        [SerializeField] private AnimationCurve featureFreq;
        [SerializeField, Min(1)] private float featureFreqPeriod;
        [Header("Случайное распределение множителя частоты (min, max)")]
        // TODO: рандомные числа нужно как то по другому получать
        [SerializeField] private Vector2 featureFreqMultiplier;
        [Header("Аниматоры фичей")]
        [SerializeField] private Animator[] featureAnimators;
        private FeatureAnimData[] featureAnimDatas;
        // Start is called before the first frame update
        private void OnEnable()
        {
            foreach (Animator A in featureAnimators) A.enabled = true;
        }
        private void OnDisable()
        {
            foreach (Animator A in featureAnimators) A.enabled = false;
        }
        private void Awake()
        {
            if (featureFreq.postWrapMode != WrapMode.Loop) Debug.LogWarning("Выставьте чтобы был Loop у featureFreq");
            InitializeFeaturePeriodDeltas();
        }

        private void InitializeFeaturePeriodDeltas()
        {
            featureAnimDatas = new FeatureAnimData[featureAnimators.Length];
            for (int i = 0; i < featureAnimators.Length; i++)
            {
                featureAnimDatas[i] = new FeatureAnimData(Time.time + Random.Range(0, featureFreqPeriod),
                    Random.Range(featureFreqMultiplier.x, featureFreqMultiplier.y));
            }
        }
        
        private void UpdateFrequencyFor(int index)
        {
            featureAnimators[index].SetFloat(HKEY_FEATURE_FREQUENCY, featureFreq.Evaluate(featureAnimDatas[index].GetTime()));
        }


        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < featureAnimators.Length; i++)
            {
                UpdateFrequencyFor(i);
            }
        }
    }

}