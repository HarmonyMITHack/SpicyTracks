using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioClip audioClip2;
    public AudioSource audioSource;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (audioClip != null)
            {
                //audioSource.PlayOneShot(audioClip);
                //AudioManager.Instance.PlaySoundAtPosition(audioClip, transform);
                MonolithicAudio.Instance.PlayAudioClip(audioClip);
            }
        }
        /*else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (audioClip2 != null)
            {
                MonolithicAudio.Instance.PlayAudioClip(null, 1f, 1.5f);
            }
        }*/
    }
}
