using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanSound : MonoBehaviour
{
    //public AudioSource audioSource;
    public AudioClip[] clips;

    public void PlayRandomSound()
    {
        MonolithicAudio.Instance.PlayUnrecordedAudioClip(clips[Random.Range(0, clips.Length)], 0.7f, Random.Range(0.95f, 1.05f));
    }
}
