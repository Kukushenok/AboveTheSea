using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerFootManager : MonoBehaviour
    {
        [Header("Параметры аудио шагов")]
        [SerializeField] private RandomAudioSource stepSound;
        [field: SerializeField, Tooltip("Критическое время, за которое состояние на 'забывается'")]
        public float critAudioTime { get; private set; } = 0.1f;
        public float velocityMultiplier { get; private set; }
        [field: Header("Параметры луча")]
        [field: SerializeField, Tooltip("Смещение по вертикали луча для посадки стопы")]
        public float rayYOffset { get; private set; } = 1;
        [field: SerializeField, Tooltip("Длина луча для посадки стопы")]
        public float rayDistance { get; private set; } = 1.1f;
        [field: SerializeField, Tooltip("Смещение посадки стопы вверх нормали поверхности")]
        public float plantedYOffset { get; private set; } = 0.1f;
        [field: SerializeField, Tooltip("Физический 'слой' объектов для посадки стопы")]
        public LayerMask mask { get; private set; }
        [field: SerializeField, Tooltip("Максимальный угол наклона стопы при посадке")]
        public float maxAngleDifference { get; private set; }
        [field: Header("Параметры лучей")]
        [field: SerializeField, Tooltip("Шаг смещения лучей относительно пятки к носку")]
        public float forwardOffset { get; private set; } = 0.15f;
        [field: SerializeField, Tooltip("Кол-во смещений лучей относительно пятки к носку")]
        public int forwardOffsetCount { get; private set; } = 4;
        [Header("Ноги"), SerializeField]
        private PlayerFootIKPlanter[] feet;
        
        // Start is called before the first frame update
        private void Awake()
        {
            foreach (PlayerFootIKPlanter foot in feet)
            {
                foot.AssignManager(this);
                foot.OnStepEvent = OnStepOnMaterial;
            }
        }
        public void LateUpdate()
        {
            foreach (PlayerFootIKPlanter foot in feet)
            {
                foot.FootIKUpdate();
            }
        }
        void OnStepOnMaterial(float velocity)
        {
            Debug.Log(velocity);
            stepSound.PlayRandomSound();
        }
    }
}