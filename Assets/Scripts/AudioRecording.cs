using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class AudioRecording : MonoBehaviour
{
    public float recordingLength = 10f;

    private string savePath;
    private int sampleRate = 48000;
    private int channels = 2;
    private List<float> audioDataList = new();
    private bool isRecording = false;

    public const string AUDIO_FILE_NAME = "audio_recording.wav";

    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, AUDIO_FILE_NAME);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (!isRecording) return;

        Debug.Log($"Audio filter read: {data.Length} samples");

        audioDataList.AddRange(data);
    }

    [ContextMenu("Start Recording")]
    public void StartRecording()
    {
        audioDataList.Clear();
        isRecording = true;
        Debug.Log("Recording started");
        StartCoroutine(TrackLength());
    }

    private IEnumerator TrackLength()
    {
        yield return new WaitForSeconds(recordingLength);
        StopRecording();
    }

    [ContextMenu("Stop Recording")]
    public void StopRecording()
    {
        if (!isRecording) return;
        isRecording = false;
        Debug.Log("Recording stopped");
        SaveAudioToWAV();
    }

    private void SaveAudioToWAV()
    {
        if (audioDataList.Count == 0)
        {
            Debug.LogWarning("No audio data to save");
            return;
        }

        byte[] wavFile = ConvertToWAV(audioDataList.ToArray(), audioDataList.Count, sampleRate, channels);
        File.WriteAllBytes(savePath, wavFile);
        Debug.Log($"Audio saved to {savePath}");
    }

    private byte[] ConvertToWAV(float[] audioData, int length, int sampleRate, int channels)
    {
        using MemoryStream stream = new();
        int byteRate = sampleRate * channels * 2;

        // WAV file header
        WriteString(stream, "RIFF");                           // Chunk ID
        WriteInt(stream, 36 + length * 2);                     // Chunk size
        WriteString(stream, "WAVE");                           // Format
        WriteString(stream, "fmt ");                           // Subchunk1 ID
        WriteInt(stream, 16);                                  // Subchunk1 size
        WriteShort(stream, 1);                                 // Audio format (PCM)
        WriteShort(stream, (short)channels);                   // Number of channels
        WriteInt(stream, sampleRate);                          // Sample rate
        WriteInt(stream, byteRate);                            // Byte rate
        WriteShort(stream, (short)(channels * 2));             // Block align
        WriteShort(stream, 16);                                // Bits per sample
        WriteString(stream, "data");                           // Subchunk2 ID
        WriteInt(stream, length * 2);                          // Subchunk2 size

        for (int i = 0; i < length; i++)
        {
            short sample = (short)(Mathf.Clamp(audioData[i], -1f, 1f) * short.MaxValue);
            WriteShort(stream, sample);
        }

        return stream.ToArray();
    }

    private void WriteString(Stream stream, string value)
    {
        foreach (char c in value)
        {
            stream.WriteByte((byte)c);
        }
    }

    private void WriteInt(Stream stream, int value)
    {
        stream.WriteByte((byte)(value & 0xFF));
        stream.WriteByte((byte)((value >> 8) & 0xFF));
        stream.WriteByte((byte)((value >> 16) & 0xFF));
        stream.WriteByte((byte)((value >> 24) & 0xFF));
    }

    private void WriteShort(Stream stream, short value)
    {
        stream.WriteByte((byte)(value & 0xFF));
        stream.WriteByte((byte)((value >> 8) & 0xFF));
    }

    public void DeleteFile()
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, AUDIO_FILE_NAME);

        if (System.IO.File.Exists(filePath))
        {
            Debug.Log("Deleted audio file");
            System.IO.File.Delete(filePath);
        }
        else
        {
            Debug.Log("Audio file does not exist");
        }
    }
}
