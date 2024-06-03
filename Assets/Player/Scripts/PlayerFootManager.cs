using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerFootManager : MonoBehaviour
    {
        [Header("��������� ����� �����")]
        [SerializeField] private RandomAudioSource stepSound;
        [field: SerializeField, Tooltip("����������� �����, �� ������� ��������� �� '����������'")]
        public float critAudioTime { get; private set; } = 0.1f;
        public float velocityMultiplier { get; private set; }
        [field: Header("��������� ����")]
        [field: SerializeField, Tooltip("�������� �� ��������� ���� ��� ������� �����")]
        public float rayYOffset { get; private set; } = 1;
        [field: SerializeField, Tooltip("����� ���� ��� ������� �����")]
        public float rayDistance { get; private set; } = 1.1f;
        [field: SerializeField, Tooltip("�������� ������� ����� ����� ������� �����������")]
        public float plantedYOffset { get; private set; } = 0.1f;
        [field: SerializeField, Tooltip("���������� '����' �������� ��� ������� �����")]
        public LayerMask mask { get; private set; }
        [field: SerializeField, Tooltip("������������ ���� ������� ����� ��� �������")]
        public float maxAngleDifference { get; private set; }
        [field: Header("��������� �����")]
        [field: SerializeField, Tooltip("��� �������� ����� ������������ ����� � �����")]
        public float forwardOffset { get; private set; } = 0.15f;
        [field: SerializeField, Tooltip("���-�� �������� ����� ������������ ����� � �����")]
        public int forwardOffsetCount { get; private set; } = 4;
        [Header("����"), SerializeField]
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
        //    Debug.Log(velocity);
            stepSound.PlayRandomSound();
        }
    }
}