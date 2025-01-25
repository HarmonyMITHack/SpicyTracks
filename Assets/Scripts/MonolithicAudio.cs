using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonolithicAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioFilePlayer audioFilePlayer;
    [SerializeField] private AudioRecording audioRecording;

    public static MonolithicAudio Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartLoop();
    }

    public void PlayAudioClip(AudioClip clip, float volume = 1f)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    [ContextMenu("Start Loop")]
    public void StartLoop()
    {
        Debug.Log("Starting loop");
        audioFilePlayer.PlayRecordedAudio();
        audioRecording.StartRecording();
    }

    [ContextMenu("Stop Loop")]
    public void StopLoop()
    {
        Debug.Log("Stopping loop");
        audioRecording.StopRecording();
    }
}
