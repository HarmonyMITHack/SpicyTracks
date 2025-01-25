using UnityEngine;
using System.Collections;

public class InterruptAudioMovement : MonoBehaviour
{
    public AudioSource audioSource;           // The AudioSource to play the music
    public AudioClip originalClip;           // The original music clip
    public AudioClip movementClip;           // The music to play during sudden movement
    public float movementThreshold = 1.0f;   // Threshold for detecting sudden movement
    public float fadeDuration = 0.1f;        // Duration for the fade effect

    private Vector3 lastPosition;
    private bool isMoving = false;

    private void Start()
    {
        // Initialize the last position
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Detect the movement by comparing the current position with the last position
        float movement = (transform.position - lastPosition).magnitude;

        if (movement > movementThreshold && !isMoving)
        {
            // If sudden movement is detected, stop the current audio and play the movement clip
            isMoving = true;
            StartCoroutine(FadeOutAndSwitchClip(movementClip)); // Fade out and switch to movement clip
        }
        else if (movement <= movementThreshold && isMoving)
        {
            // If movement stops, stop the current audio and play the original clip
            isMoving = false;
            StartCoroutine(FadeOutAndSwitchClip(originalClip)); // Fade out and switch to original clip
        }

        // Update the last position
        lastPosition = transform.position;
    }

    private IEnumerator FadeOutAndSwitchClip(AudioClip newClip)
    {
        // Fade out the current audio over the specified duration
        float startVolume = audioSource.volume;

        // Fade out
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the volume is fully zero and stop the audio
        audioSource.volume = 0f;
        audioSource.Stop();

        // Change the clip and play it
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in the new clip
        timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration); // Fade to full volume
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the volume is fully restored to 1
        audioSource.volume = 1f;
    }
}
