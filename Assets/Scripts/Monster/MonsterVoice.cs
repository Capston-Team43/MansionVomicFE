using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterVoice : MonoBehaviour
{
    //private string voiceApiUrl = "http://20.41.115.23:8000/latest-tts-audio/";
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Cannot find the AudioSource.");
            return;
        }

        //PlayFromServer(voiceApiUrl);
    }

    public void PlayFromServer(string apiUrl)
    {
        //StartCoroutine(DownloadAndPlayWav(apiUrl));
        StartCoroutine(DownloadAndPlayMp3(apiUrl));
    }

    IEnumerator DownloadAndPlayMp3(string url)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Monster voice failed: " + request.error);
            yield break;
        }

        AudioClip clip = DownloadHandlerAudioClip.GetContent(request);

        Debug.Log($"Download success - file size: {clip.length},{clip.channels}");

        // 2D 사운드
        audioSource.spatialBlend = 0f;
        audioSource.clip = clip;
        audioSource.Play();

        Debug.Log("Monster voice success.");
    }

    /*
    IEnumerator DownloadAndPlayWav(string url)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Monster voice failed: " + request.error);
            yield break;
        }

        AudioClip clip = DownloadHandlerAudioClip.GetContent(request);

        Debug.Log($"Download success - file size: {clip.length},{clip.channels}");

        // 2D 사운드
        audioSource.spatialBlend = 0f;
        audioSource.clip = clip;
        audioSource.Play();

        Debug.Log("Monster voice success.");
    }
    */
}
