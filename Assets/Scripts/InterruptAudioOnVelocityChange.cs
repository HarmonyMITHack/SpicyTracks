using UnityEngine;
using System.Collections;

public class InterruptAudioMovement : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip originalClip;
    public AudioClip movementClip;
    public float movementThreshold = 1.0f;
    public float fadeDuration = 0.1f;

    private Coroutine currentFade;
    private Vector3 lastPosition;
    private bool isMoving = false;
    private bool letGo = false;

    private void Start()
    {
        lastPosition = transform.position;

    }

    private void Update()
    {
        float movement = (transform.position - lastPosition).magnitude;

        if (letGo)
        {
            StopCurrentFade();
            PlayClip(originalClip);
            letGo = false;
            lastPosition = transform.position;
            return;
        }

        if (movement > movementThreshold && !isMoving)
        {
            isMoving = true;
            StopCurrentFade();
            currentFade = StartCoroutine(FadeOutAndSwitchClip(movementClip));
        }
        else if (movement <= movementThreshold && isMoving)
        {
            isMoving = false;
            StopCurrentFade();
            currentFade = StartCoroutine(FadeOutAndSwitchClip(originalClip));
        }

        lastPosition = transform.position;
    }

    private IEnumerator FadeOutAndSwitchClip(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        // Fade out current audio
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0f;

        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in new audio
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 1f;
    }

    public void Released()
    {
        letGo = true;
    }

    private void StopCurrentFade()
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
            currentFade = null;
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.volume = 0f;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
