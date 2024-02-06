using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSource : MonoBehaviour
{
    [Header("������������� ��������������� ��������� ����� �� ������.")]
    private AudioSource source;
    [SerializeField, Tooltip("��������� �����")] private AudioClip[] clips;
    int previousClipIdx;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        previousClipIdx = Random.Range(0, clips.Length);
    }
    public void PlayRandomSound()
    {
        previousClipIdx = (previousClipIdx + Random.Range(1, clips.Length)) % clips.Length;
        source.PlayOneShot(clips[previousClipIdx]);
    }
}
