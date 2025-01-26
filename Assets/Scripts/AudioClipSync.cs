using UnityEngine;

public class AudioClipSync : MonoBehaviour
{
    public AudioSource audioSource;

    /// <summary>
    /// Replaces the currently playing AudioClip with a new one and syncs its timing.
    /// </summary>
    /// <param name="newClip">The new AudioClip to play.</param>
    public void PlayClipAtSameTime(AudioClip newClip)
    {
        if (audioSource == null || newClip == null)
        {
            Debug.LogWarning("AudioSource or new AudioClip is null.");
            return;
        }

        if (audioSource.isPlaying)
        {
            float currentTime = audioSource.time;
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();
            audioSource.time = Mathf.Min(currentTime, newClip.length);
        }
        else
        {
            // If nothing is playing, just play the new clip normally.
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}
