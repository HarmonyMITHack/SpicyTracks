using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioFilePlayer : MonoBehaviour
{
    string audioFileName = AudioRecording.AUDIO_FILE_NAME;

    /*private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Test();
        }
    }*/

    public void PlayRecordedAudio()
    {
        StartCoroutine(LoadAndPlayAudio());
    }

    private IEnumerator LoadAndPlayAudio()
    {
        string filePath = "file://" + System.IO.Path.Combine(Application.persistentDataPath, audioFileName);

        Debug.Log("FilePath: " + filePath);

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);

            MonolithicAudio.Instance.PlayAudioClip(clip);
        }
        else
        {
            Debug.LogWarning("Audio file not loaded " + request.result);
        }
    }
/*
    public void WriteToFile(string fileName, string content)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.WriteAllText(filePath, content);
            Debug.Log($"File written successfully at: {filePath}");
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to write file: {ex.Message}");
        }
    }

    public void ReadFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            Debug.Log($"File content: {content}");
        }
        else
        {
            Debug.LogError($"File not found at: {filePath}");
        }
    }

    private void Test()
    {
        string fileName = "example.txt";
        string content = "This is a test file";

        WriteToFile(fileName, content);

        ReadFromFile(fileName);
    }*/
}
