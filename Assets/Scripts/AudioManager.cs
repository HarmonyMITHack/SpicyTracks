using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerGroup;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource audioSource;

    private const string VOLUME_PARAM = "Volume";

    private List<AudioSource> audioSourcePool;
    private readonly int initialPoolSize = 10;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeAudioSourcePool();
        }
        else
        {
            Destroy(this);
        }
    }

    private void InitializeAudioSourcePool()
    {
        audioSourcePool = new List<AudioSource>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            audioSourcePool.Add(CreateNewAudioSource());
        }
    }

    private AudioSource GetPooledAudioSource()
    {
        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }

        // If all sources are playing, expand the pool
        Debug.Log("Expanding audio source pool");
        AudioSource newSource = CreateNewAudioSource();
        audioSourcePool.Add(newSource);
        return newSource;
    }

    private AudioSource CreateNewAudioSource()
    {
        GameObject tempGO = new("PooledAudioSource");
        tempGO.transform.SetParent(transform);
        AudioSource tempAudioSource = tempGO.AddComponent<AudioSource>();
        tempAudioSource.playOnAwake = false;
        tempAudioSource.outputAudioMixerGroup = mixerGroup;
        tempGO.SetActive(false);
        return tempAudioSource;
    }

    private void CrossfadeMixer(float to, float duration)
    {
        audioMixer.DOSetFloat(VOLUME_PARAM, to, duration);
    }

    private void Crossfade(float toVolume, float duration)
    {
        DOTween.To(() => audioSource.volume, x => audioSource.volume = x, toVolume, duration);
    }

    public static void Crossfade(AudioSource source, float toVolume, float duration)
    {
        DOTween.To(() => source.volume, x => source.volume = x, toVolume, duration);
    }

    [ContextMenu("Crossfade Out")]
    private void CrossfadeOut()
    {
        Crossfade(0f, 2f);
    }

    [ContextMenu("Crossfade In")]
    private void CrossfadeIn()
    {
        Crossfade(1f, 2f);
    }

    public void PlaySoundAtPosition(AudioClip soundClip, Transform transform, float volume = 1f, float pitch = 1f)
    {
        if (soundClip != null)
        {
            AudioSource tempAudioSource = GetPooledAudioSource();
            tempAudioSource.transform.position = transform.position;
            tempAudioSource.clip = soundClip;
            tempAudioSource.volume = volume * (1 / pitch); // I think this normalizes the volume after pitch adjustment
            tempAudioSource.pitch = pitch;
            tempAudioSource.PlayOneShot(soundClip);
        }
    }
}
