using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioFilePlayer : MonoBehaviour
{
    string audioFileName = AudioRecording.AUDIO_FILE_NAME; 

    public void PlayRecordedAudio()
    {
        StartCoroutine(LoadAndPlayAudio());
    }

    private IEnumerator LoadAndPlayAudio()
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, audioFileName);

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);

            MonolithicAudio.Instance.PlayAudioClip(clip);
        }
        else
        {
            Debug.LogWarning("Audio file not loaded");
        }
    }
}
