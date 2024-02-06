using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSource : MonoBehaviour
{
    [Header("¬оспроизводит неповтор€ющиес€ случайные клипы из списка.")]
    private AudioSource source;
    [SerializeField, Tooltip("–андомные клипы")] private AudioClip[] clips;
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
