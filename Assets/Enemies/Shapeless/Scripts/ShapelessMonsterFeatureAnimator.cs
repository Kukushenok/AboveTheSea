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
        const string HKEY_FEATURE_MAIN = "feature_main";
        [Header("Распределения частоты фичей по времени (от 0 до 1)")]
        [SerializeField] private float featureFreq;
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
            InitializeFeaturePeriodDeltas();
        }

        private void InitializeFeaturePeriodDeltas()
        {
            featureAnimDatas = new FeatureAnimData[featureAnimators.Length];
            for (int i = 0; i < featureAnimators.Length; i++)
            {
                featureAnimDatas[i] = new FeatureAnimData(0, 1);//+ Random.Range(0, featureFreqPeriod),
                                                                //Random.Range(featureFreqMultiplier.x, featureFreqMultiplier.y));
                // Random offset.
                featureAnimators[i].Play(HKEY_FEATURE_MAIN, 0, Random.Range(0.0f, 1.0f));
            }
        }
        
        private void UpdateFrequencyFor(int index)
        {
            featureAnimators[index].SetFloat(HKEY_FEATURE_FREQUENCY, featureFreq);
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