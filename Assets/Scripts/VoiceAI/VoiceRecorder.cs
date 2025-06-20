using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

public class VoiceRecorder : MonoBehaviour
{
    public int recordTimeSeconds = 5;
    public string fileName = "voice_sample.wav";
    private AudioClip recordedClip;

    string deviceName;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            deviceName = Microphone.devices[0];
            Debug.Log("Using mic: " + deviceName);
        }
        else
        {
            Debug.LogWarning("No microphone devices found!");
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 사라지지 않음
    }

    public void StartRecording()
    {
        Debug.Log("Start Recording");
        recordedClip = Microphone.Start(null, false, recordTimeSeconds, 44100);
        Invoke(nameof(StopRecording), recordTimeSeconds);
    }

    void StopRecording()
    {
        Debug.Log("Finish Recording");
        Microphone.End(null);

        // AudioClip -> WAV 변환 -> 바이트 저장
        byte[] wavData = WavUtility.FromAudioClip(recordedClip, out string filepath, true);
        Debug.Log($"저장된 파일 경로: {filepath}");

        // API 전송
        StartCoroutine(UploadToAzure(filepath));
    }

    IEnumerator UploadToAzure(string path)
    {
        byte[] data = File.ReadAllBytes(path);

        // Create form
        WWWForm form = new WWWForm();
        //form.AddBinaryData("audio", data, "recording.wav", "audio/wav");
        form.AddBinaryData("file", data, "recording.wav", "audio/wav");


        UnityWebRequest req = UnityWebRequest.Post("http://20.41.115.23:8000/register-voice/", form);
        Debug.Log("Sending to: /register-voice/");

        /*
        UnityWebRequest req = UnityWebRequest.Put("API", data);
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.SetRequestHeader("Content-Type", "audio/wav");
        */

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log("STT request transmission successed");
        else
            Debug.LogError($"STT request transmission failed: {req.error}");
    }
}