using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMusic : MonoBehaviour
{
    [SerializeField] private AudioClip clip1, clip2;
    [SerializeField] private AudioSource source1, source2;
    [SerializeField] private float fadeDuration = 0.2f;
    private State currentState = State.None;

    private enum State
    {
        None = 0,
        Track1 = 1,
        Track2 = 2,
    }

    public void OnPickup()
    {
        AudioManager.Crossfade(source1, 1f, fadeDuration);
        source2.volume = 0f;
    }

    public void OnRelease()
    {
        AudioManager.Crossfade(source1, 0f, fadeDuration);
        AudioManager.Crossfade(source2, 0f, fadeDuration);
    }

    public void FadeTo1(float duration)
    {

        AudioManager.Crossfade(source1, 1f, duration);
        AudioManager.Crossfade(source2, 0f, duration);
        currentState = State.Track1;
    }

    public void FadeTo2(float duration)
    {
        AudioManager.Crossfade(source1, 0f, duration);
        AudioManager.Crossfade(source2, 1f, duration);
        currentState = State.Track2;
    }

    public void TryFade1()
    {
        if (currentState != State.Track1)
        {
            FadeTo1(fadeDuration);
        }
    }

    public void TryFade2()
    {
        if (currentState != State.Track2)
        {
            FadeTo2(fadeDuration);
        }
    }
}
