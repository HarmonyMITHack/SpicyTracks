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
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.One))
        {
            if (audioClip != null)
            {
                MonolithicAudio.Instance.PlayAudioClip(audioClip);
            }
        }
    }
}
