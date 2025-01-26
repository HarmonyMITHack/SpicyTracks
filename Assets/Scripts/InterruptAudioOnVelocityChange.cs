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
            if (currentFade != null)
            {
                currentFade = StartCoroutine(FadeOutAndSwitchClip(originalClip));
                lastPosition = transform.position;
                letGo = false;
                return;
            }
        }

        if (movement > movementThreshold && !isMoving)
        {
            isMoving = true;

            if (currentFade != null)
            {
                StopCoroutine(currentFade);
            }

            currentFade = StartCoroutine(FadeOutAndSwitchClip(movementClip));
        }
        else if (movement <= movementThreshold && isMoving)
        {
            isMoving = false;

            if (currentFade != null)
            {
                StopCoroutine(currentFade);
            }

            currentFade = StartCoroutine(FadeOutAndSwitchClip(originalClip));
        }

        lastPosition = transform.position;
    }

    private IEnumerator FadeOutAndSwitchClip(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        audioSource.clip = newClip;
        audioSource.Play();

        timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 1f;
    }

    public void Released()
    {
        letGo = true;

        /*if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        currentFade = StartCoroutine(FadeOutAndSwitchClip(originalClip));*/
    }
}
